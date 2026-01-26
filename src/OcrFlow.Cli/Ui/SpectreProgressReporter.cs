using System.Collections.Concurrent;
using Spectre.Console;

namespace OcrFlow.Cli.Ui;

public sealed class SpectreProgressReporter : IProgressReporter, IDisposable
{
    private readonly CancellationTokenSource _cts = new();
    private readonly ConcurrentDictionary<int, PageStatus> _pages = new();
    private Task? _uiTask;

    private int _disposed; // 0 = alive, 1 = disposed

    public void Dispose()
    {
        if (Interlocked.Exchange(ref _disposed, 1) == 1)
            return;

        if (!_cts.IsCancellationRequested)
            _cts.Cancel();

        try
        {
            _uiTask?.Wait();
        }
        finally
        {
            _cts.Dispose();
        }
    }

    public void PageStarted(int pageNo, string file)
    {
        _pages[pageNo] = new PageStatus
        {
            PageNo = pageNo,
            File = Path.GetFileName(file),
            Status = "[yellow]running[/]"
        };
    }

    public void PageCompleted(int pageNo, long ocrMs)
    {
        if (_pages.TryGetValue(pageNo, out var p))
        {
            p.Status = "[green]done[/]";
            p.OcrMs = ocrMs;
        }
    }

    public IReadOnlyCollection<PageStatus> Snapshot()
    {
        return _pages.Values.ToList();
    }

    public void RunUi()
    {
        AnsiConsole.Live(BuildTable())
            .AutoClear(false)
            .Start(ctx =>
            {
                _uiTask = Task.Run(async () =>
                {
                    while (!_cts.IsCancellationRequested)
                    {
                        ctx.UpdateTarget(BuildTable());
                        await Task.Delay(100);
                    }
                });

                // blokada aż Dispose()
                _ = _cts.Token.WaitHandle.WaitOne();
            });
    }

    public void PageFailed(int pageNo, string error)
    {
        if (_pages.TryGetValue(pageNo, out var p))
            p.Status = $"[red]error[/] {error}";
    }

    private Table BuildTable()
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("#")
            .AddColumn("File")
            .AddColumn("Status")
            .AddColumn("OCR [[ms]]");

        foreach (var p in _pages.Values.OrderBy(x => x.PageNo))
            _ = table.AddRow(
                p.PageNo.ToString(),
                p.File,
                p.Status,
                p.OcrMs == 0 ? "-" : p.OcrMs.ToString());

        return table;
    }
}