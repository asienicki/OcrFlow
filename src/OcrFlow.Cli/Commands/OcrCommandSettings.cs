using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace OcrFlow.Cli.Commands;

/// <summary>
///     Settings for the OCR CLI command.
///     Defines all supported command-line arguments and options
///     and performs basic validation before execution.
/// </summary>
public sealed class OcrCommandSettings : CommandSettings
{
#if DEBUG
    /// <summary>
    ///     Forces an exception for debugging and error-handling tests.
    ///     Available only in DEBUG builds.
    /// </summary>
    [CommandOption("--throw")]
    [Description("Force an exception (DEBUG only)")]
    public bool ThrowException { get; init; }
#endif

    /// <summary>
    ///     Input directory passed as a positional argument.
    /// </summary>
    [CommandArgument(0, "[inputDir]")]
    [Description("Input directory containing images (positional argument)")]
    public string? InputDirArg { get; init; }

    /// <summary>
    ///     Input directory passed as a named option.
    ///     Takes precedence over positional argument.
    /// </summary>
    [CommandOption("--source <DIR>")]
    [Description("Input directory containing images (overrides positional argument)")]
    public string? InputDirOption { get; init; }

    /// <summary>
    ///     Resolved input directory.
    ///     Uses --source if provided, otherwise positional argument.
    /// </summary>
    public string InputDir => InputDirOption ?? InputDirArg!;

    /// <summary>
    ///     OCR languages (comma-separated, e.g. eng,pol).
    ///     Passed directly to the OCR engine.
    /// </summary>
    [CommandOption("--lang <LANG>")]
    [Description("OCR languages (comma-separated, e.g. eng,pol)")]
    [DefaultValue("eng")]
    public string Lang { get; init; } = "eng";

    /// <summary>
    ///     Disables merging OCR results into a single PDF.
    ///     Each input produces a separate PDF.
    /// </summary>
    [CommandOption("--nomerge")]
    [Description("Do not merge results into a single PDF (generate separate PDFs)")]
    public bool NoMerge { get; init; }

    /// <summary>
    ///     Disables Markdown output generation.
    /// </summary>
    [CommandOption("--nomarkdown")]
    [Description("Do not generate Markdown output")]
    public bool NoMarkdown { get; init; }

    /// <summary>
    ///     Generates only Markdown output.
    ///     Skips PDF generation for better performance.
    /// </summary>
    [CommandOption("--onlymarkdown")]
    [Description("Generate only Markdown output (skip PDF generation)")]
    public bool OnlyMarkdown { get; init; }

    /// <summary>
    ///     Disables generation of the final result/summary file.
    /// </summary>
    [CommandOption("--noresult")]
    [Description("Do not generate the final result/summary file")]
    public bool NoResult { get; init; }

    /// <summary>
    ///     Disables PDF generation.
    /// </summary>
    [CommandOption("--nopdf")]
    [Description("Do not generate PDF output")]
    public bool NoPdf { get; init; }

    /// <summary>
    ///     Generates only PDF output.
    ///     Skips Markdown generation.
    /// </summary>
    [CommandOption("--onlypdf")]
    [Description("Generate only PDF output (skip Markdown generation)")]
    public bool OnlyPdf { get; init; }

    /// <summary>
    ///     Output directory for all generated files.
    /// </summary>
    [CommandOption("--outputdir <DIR>")]
    [Description("Output directory for all generated files (defaults to input directory)")]
    public string OutputDir { get; internal set; } = null!;

    /// <summary>
    ///     Prefix used when generating Markdown file names.
    /// </summary>
    [CommandOption("--md-name-prefix <TEXT>")]
    [Description("Prefix added to generated Markdown file names")]
    public string MarkdownFileNamePrefix { get; internal set; } = null!;

    /// <summary>
    ///     Prefix used when generating PDF file names.
    /// </summary>
    [CommandOption("--pdf-name-prefix <TEXT>")]
    [Description("Prefix added to generated PDF file names")]
    public string PdfFileNamePrefix { get; internal set; } = null!;

    /// <summary>
    ///     Validates command-line arguments and option combinations.
    /// </summary>
    public override ValidationResult Validate()
    {
        if (string.IsNullOrWhiteSpace(InputDir))
            return ValidationResult.Error("Input directory is required");

        if (!Directory.Exists(InputDir))
            return ValidationResult.Error($"Directory does not exist: {InputDir}");

        if (NoMarkdown && OnlyMarkdown)
            return ValidationResult.Error(
                "\nOptions:\n\t--nomarkdown \nand\n\t--onlymarkdown\ncannot be used together.");

        return NoPdf && OnlyPdf
            ? ValidationResult.Error(
                "\nOptions:\n\t--nopdf \nand\n\t--onlypdf\ncannot be used together.")
            : ValidationResult.Success();
    }
}
