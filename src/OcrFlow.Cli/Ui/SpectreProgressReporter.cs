using Spectre.Console;
using System.Collections.Concurrent;

public interface IProgressReporter
{
    void PageStarted(int pageNo, string file);
    void PageCompleted(int pageNo, long ocrMs);
}
public sealed class PageStatus
{
    public int PageNo { get; init; }
    public string File { get; init; } = "";
    public string Status { get; set; } = "pending";
    public long OcrMs { get; set; }
}

public sealed class SpectreProgressReporter : IProgressReporter, IDisposable
{
    private readonly ConcurrentDictionary<int, PageStatus> _pages = new();
    private readonly CancellationTokenSource _cts = new();
    private Task? _uiTask;
    public IReadOnlyCollection<PageStatus> Snapshot()
    => _pages.Values.ToList();

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
                _cts.Token.WaitHandle.WaitOne();
            });
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
            table.AddRow(
                p.PageNo.ToString(),
                p.File,
                p.Status,
                p.OcrMs == 0 ? "-" : p.OcrMs.ToString());

        return table;
    }

    public void Dispose()
    {
        _cts.Cancel();
        _uiTask?.Wait();
    }
}
