using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace OcrFlow.Cli.Commands;

public sealed class OcrCommandSettings : CommandSettings
{
#if DEBUG
    [CommandOption("--throw")]
    public bool ThrowException { get; init; }
#endif

    [Description("Input directory containing images")]
    [CommandArgument(0, "[inputDir]")]
    public string? InputDirArg { get; init; }

    [CommandOption("--source <DIR>")]
    [Description("Input directory containing images")]
    public string? InputDirOption { get; init; }

    public string InputDir => InputDirOption ?? InputDirArg;

    [Description("OCR languages (e.g. eng, pol)")]
    [CommandOption("--lang <LANG>")]
    [DefaultValue("eng")]
    public string Lang { get; init; } = "eng";

    [Description("Do not merge results (separate PDFs)")]
    [CommandOption("--nomerge")]
    public bool NoMerge { get; init; }

    [Description("Do not generate Markdown output")]
    [CommandOption("--nomarkdown")]
    public bool NoMarkdown { get; init; }

    [Description("Generate only Markdown output (skip PDF generation; slightly faster)")]
    [CommandOption("--onlymarkdown")]
    public bool OnlyMarkdown { get; init; }

    [Description("Do not generate the result summary file")]
    [CommandOption("--noresult")]
    public bool NoResult { get; init; }

    [CommandOption("--nopdf")]
    public bool NoPdf { get; init; }

    [CommandOption("--onlypdf")]
    public bool OnlyPdf { get; init; }

    [CommandOption("--outputdir")]
    public string OutputDir { get; internal set; }

    [CommandOption("--pdf-name-prefix")]
    public string MarkdownFileNamePrefix { get; internal set; }

    [CommandOption("--md-name-prefix")]
    public string PdfFileNamePrefix { get; internal set; }

    public override ValidationResult Validate()
    {
        if (string.IsNullOrWhiteSpace(InputDir))
            return ValidationResult.Error("Input directory is required");

        if (!Directory.Exists(InputDir))
            return ValidationResult.Error($"Directory does not exist: {InputDir}");

        if (NoMarkdown && OnlyMarkdown)
            return ValidationResult.Error(
                "\nOptions:\n\t--nomarkdown \nand\n\t--onlymarkdown\ncannot be used together."
            );

        if (NoPdf && OnlyPdf)
            return ValidationResult.Error(
                "\nOptions:\n\t--nopdf \nand\n\t--onlypdf\ncannot be used together."
            );


        return ValidationResult.Success();
    }
}
