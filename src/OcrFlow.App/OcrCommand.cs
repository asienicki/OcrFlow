using Spectre.Console.Cli;
using Spectre.Console;
using System.ComponentModel;

public sealed class OcrCommand : Command<OcrCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [Description("Katalog wejściowy ze zdjęciami")]
        [CommandArgument(0, "<inputDir>")]
        public string InputDir { get; init; } = "";

        [Description("Języki OCR (np. eng,pol)")]
        [CommandOption("--lang <LANG>")]
        [DefaultValue("eng")]
        public string Lang { get; init; } = "eng";

        [Description("Scal wynik do jednego PDF")]
        [CommandOption("--merge")]
        public bool Merge { get; init; }

        public override ValidationResult Validate()
        {
            if (string.IsNullOrWhiteSpace(InputDir))
                return ValidationResult.Error("InputDir jest wymagany");

            if (!Directory.Exists(InputDir))
                return ValidationResult.Error($"Katalog nie istnieje: {InputDir}");

            return ValidationResult.Success();
        }
    }

    public override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var options = new CliOptions
        {
            InputDir = settings.InputDir,
            Lang = settings.Lang,
            Merge = settings.Merge
        };

        using var reporter = new SpectreProgressReporter();

        var uiTask = Task.Run(() => reporter.RunUi());

        OcrPipeline.Run(options, new OcrSettings(), reporter);

        reporter.Dispose();
        uiTask.Wait();

        AnsiConsole.MarkupLine("[green]Gotowe[/]");
        var resultPath = Path.Combine(options.InputDir, $"result-{DateTime.Now:yyyy-MM-dd_HH_mm}.md");

        TableMarkdownExporter.Save(
            resultPath,
            reporter.Snapshot()
        );

        return 0;
    }
}