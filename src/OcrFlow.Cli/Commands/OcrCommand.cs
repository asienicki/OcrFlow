using OcrFlow.Application.Services;
using OcrFlow.Cli.Bootstrap;
using OcrFlow.Cli.Ui;
using OcrFlow.Core.Flow.Models;
using OcrFlow.Core.Output.Abstractions;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics;

namespace OcrFlow.Cli.Commands;

public sealed class OcrCommand : AsyncCommand<OcrCommandSettings>
{
    private readonly IOcrApplicationService _ocr;
    private readonly OcrRunContext _context;
    private readonly OcrRunOptionsBuilder _builder;
    private readonly IEnumerable<IOutputFinalizer> _finalizers;

    public OcrCommand(
        IOcrApplicationService ocr,
        OcrRunContext context,
        OcrRunOptionsBuilder builder,
        IEnumerable<IOutputFinalizer> finalizers)
    {
        _ocr = ocr;
        _context = context;
        _builder = builder;
        _finalizers = finalizers;
    }

    public override async Task<int> ExecuteAsync(
        CommandContext context,
        OcrCommandSettings settings,
        CancellationToken ct)
    {
        var options = _builder.Build(settings);
        _context.Initialize(options);
        OcrOptionsPrinter.Print(options);

        await ProcessFilesAsync(options, ct);

        foreach (var finalizer in _finalizers)
        {
            if (finalizer.ShouldRun())
                await finalizer.FinalizeAsync(ct);
        }

        return 0;
    }

    private async Task ProcessFilesAsync(
        OcrRunOptions options,
        CancellationToken ct)
    {
        var files = Directory
            .EnumerateFiles(options.InputDir)
            .Where(f => options.ImageExtensions.Contains(Path.GetExtension(f)))
            .Select((file, index) => (file, pageNo: index + 1))
            .ToList();

        using var reporter = new SpectreProgressReporter();
        var uiTask = Task.Run(reporter.RunUi);

        try
        {
            await Parallel.ForEachAsync(
                files,
                new ParallelOptions
                {
                    MaxDegreeOfParallelism = Math.Max(1, Environment.ProcessorCount - 1),
                    CancellationToken = ct
                },
                async (item, token) =>
                {
                    await ProcessSingleFileAsync(
                        item.file,
                        item.pageNo,
                        options,
                        token,
                        reporter);
                });
        }
        finally
        {
            reporter.Dispose();
            await uiTask;
        }
    }

    private async Task ProcessSingleFileAsync(
        string file,
        int pageNo,
        OcrRunOptions options,
        CancellationToken ct,
        SpectreProgressReporter reporter)
    {
        var sw = Stopwatch.StartNew();
        reporter.PageStarted(pageNo, file);

        try
        {
            var input = new OcrInput
            {
                ImagePath = file,
                Languages = options.Languages
            };

            await _ocr.ProcessAsync(input, ct);

            reporter.PageCompleted(pageNo, sw.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            reporter.PageFailed(pageNo, ex.Message);
        }
    }
}
