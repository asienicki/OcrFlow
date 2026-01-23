using System.Diagnostics;
using System.Runtime.InteropServices;
using Tesseract;

public static class OcrPipeline
{
    public static PipelineContext PerformOcrForAllFiles(
        CliOptions cliOptions,
        OcrSettings settings,
        IProgressReporter reporter)
    {
        var inputDir = cliOptions.InputDir;
        var outputDir = Path.Combine(inputDir, settings.OutputPdfDirName);
        Directory.CreateDirectory(outputDir);

        var pipelineCtx = new PipelineContext
        {
            Options = new PipelineOptions
            {
                MergePdf = cliOptions.Merge
            },
            InputDir = inputDir,
            OutputPdfDir = outputDir
        };

        var images = GetImages(inputDir, settings);

        RunPagesParallel(images, pipelineCtx, settings, reporter);

        return pipelineCtx;
    }
    private static string[] GetImages(string inputDir, OcrSettings settings)
    => settings.ImageSearchPatterns
        .SelectMany(p => Directory.GetFiles(inputDir, p))
        .ToArray();

    public static void Run(CliOptions options, OcrSettings settings, IProgressReporter reporter)
    {
        var pipelineCtx = PerformOcrForAllFiles(options, settings, reporter);

        var tpp = new TextProcessingPipeline(
            new RomanNumeralProcessor(),
            new TextNormalizationProcessor(),
            new SpellCorrectionProcessor(
                Path.Combine(AppContext.BaseDirectory, settings.DictionaryPath))
        );

        IPipelineStep<PipelineContext>[] globalSteps =
        {
            new PdfMergeStep(),
            new MarkdownExportStep(tpp)
        };

        foreach (var step in globalSteps)
        {
            step.Execute(pipelineCtx);
        }
    }

    private static void RunPagesParallel(
        string[] images,
        PipelineContext pipelineCtx,
        OcrSettings settings,
        IProgressReporter reporter)
    {
        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = Math.Max(1, Environment.ProcessorCount - 1)
        };


        Parallel.ForEach(
            images.Select((path, index) => (path, index)),
            parallelOptions,
            item =>
            {
                NativeConsole.MuteStderr();

                var sw = new Stopwatch();

                sw.Start();

                reporter.PageStarted(item.index + 1, item.path);

                using var engine = CreateEngine(settings);


                var pagePipeline = new PagePipeline(
                    new OcrStep(),
                    new PdfBuildStep(),
                    new CollectTextStep(pipelineCtx) // MUSI być thread-safe
                );

                var pageCtx = new PageContext
                {
                    PageNo = item.index + 1,
                    ImagePath = item.path,
                    Engine = engine,
                    OutputPdfDir = pipelineCtx.OutputPdfDir
                };

                pagePipeline.Run(pageCtx);

                reporter.PageCompleted(item.index + 1, sw.ElapsedMilliseconds);
            });
    }

    private static TesseractEngine CreateEngine(OcrSettings settings)
    {
        var tessPath = Path.Combine(AppContext.BaseDirectory, settings.TessDataDir);

        var engine = new TesseractEngine(
            tessPath,
            settings.OcrLanguage,
            settings.EngineMode);

        DisableTesseractLogs(engine);

        return engine;
    }

    private static void DisableTesseractLogs(TesseractEngine engine)
    {
        var nullDevice =
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? "nul"
                : "/dev/null";

        engine.SetVariable("debug_file", nullDevice);
    }
}
