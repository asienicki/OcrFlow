using System.Diagnostics;
using OcrFlow.Application.Services;
using OcrFlow.Cli.Bootstrap;
using OcrFlow.Cli.Ui;
using OcrFlow.Core.Flow.Models;
using OcrFlow.Core.Flow.Models.Runtime;
using OcrFlow.Core.Output.Abstractions;
using Spectre.Console.Cli;

namespace OcrFlow.Cli.Commands;

public sealed class OcrCommand : AsyncCommand<OcrCommandSettings>
{
    private readonly OcrRunOptionsBuilder _builder;
    private readonly OcrRunContext _context;
    private readonly IEnumerable<IOutputFinalizer> _finalizers;
    private readonly IOcrApplicationService _ocr;

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
        CancellationToken cancellationToken)
    {
        var options = _builder.Build(settings);
        _context.Initialize(options);
        OcrOptionsPrinter.Print(options);

        await ProcessFilesAsync(options, cancellationToken);

        foreach (var finalizer in _finalizers.Where(x => x.ShouldRun()))
            await finalizer.FinalizeAsync(cancellationToken);

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
        var uiTask = Task.Run(reporter.RunUi, ct);

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
                        reporter,
                        token);
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
        SpectreProgressReporter reporter,
        CancellationToken ct)
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

            _ = await _ocr.ProcessAsync(input, ct);

            reporter.PageCompleted(pageNo, sw.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            reporter.PageFailed(pageNo, ex.Message);
        }
    }
}