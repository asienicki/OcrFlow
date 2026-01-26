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
    public bool ThrowException { get; init; }
#endif

    /// <summary>
    ///     Input directory passed as a positional argument.
    /// </summary>
    [Description("Input directory containing images")]
    [CommandArgument(0, "[inputDir]")]
    public string? InputDirArg { get; init; }

    /// <summary>
    ///     Input directory passed as a named option.
    ///     Takes precedence over positional argument.
    /// </summary>
    [CommandOption("--source <DIR>")]
    [Description("Input directory containing images")]
    public string? InputDirOption { get; init; }

    /// <summary>
    ///     Resolved input directory.
    ///     Uses --source if provided, otherwise positional argument.
    /// </summary>
    public string InputDir => InputDirOption ?? InputDirArg!;

    /// <summary>
    ///     OCR languages (comma-separated, e.g. eng, pol).
    ///     Passed directly to the OCR engine.
    /// </summary>
    [Description("OCR languages (e.g. eng, pol)")]
    [CommandOption("--lang <LANG>")]
    [DefaultValue("eng")]
    public string Lang { get; init; } = "eng";

    /// <summary>
    ///     Disables merging OCR results into a single PDF.
    ///     Each input produces a separate PDF.
    /// </summary>
    [Description("Do not merge results (separate PDFs)")]
    [CommandOption("--nomerge")]
    public bool NoMerge { get; init; }

    /// <summary>
    ///     Disables Markdown output generation.
    /// </summary>
    [Description("Do not generate Markdown output")]
    [CommandOption("--nomarkdown")]
    public bool NoMarkdown { get; init; }

    /// <summary>
    ///     Generates only Markdown output.
    ///     Skips PDF generation for better performance.
    /// </summary>
    [Description("Generate only Markdown output (skip PDF generation; slightly faster)")]
    [CommandOption("--onlymarkdown")]
    public bool OnlyMarkdown { get; init; }

    /// <summary>
    ///     Disables generation of the final result/summary file.
    /// </summary>
    [Description("Do not generate the result summary file")]
    [CommandOption("--noresult")]
    public bool NoResult { get; init; }

    /// <summary>
    ///     Disables PDF generation.
    /// </summary>
    [CommandOption("--nopdf")]
    public bool NoPdf { get; init; }

    /// <summary>
    ///     Generates only PDF output.
    ///     Skips Markdown generation.
    /// </summary>
    [CommandOption("--onlypdf")]
    public bool OnlyPdf { get; init; }

    /// <summary>
    ///     Output directory for all generated files.
    /// </summary>
    [CommandOption("--outputdir")]
    public string OutputDir { get; internal set; } = null!;

    /// <summary>
    ///     Prefix used when generating Markdown file names.
    /// </summary>
    [CommandOption("--pdf-name-prefix")]
    public string MarkdownFileNamePrefix { get; internal set; } = null!;

    /// <summary>
    ///     Prefix used when generating PDF file names.
    /// </summary>
    [CommandOption("--md-name-prefix")]
    public string PdfFileNamePrefix { get; internal set; } = null!;

    /// <summary>
    ///     Validates command-line arguments and option combinations.
    /// </summary>
    public override ValidationResult Validate()
    {
        // Input directory must be provided
        if (string.IsNullOrWhiteSpace(InputDir))
            return ValidationResult.Error("Input directory is required");

        // Input directory must exist
        if (!Directory.Exists(InputDir))
            return ValidationResult.Error($"Directory does not exist: {InputDir}");

        // Mutually exclusive Markdown options
        if (NoMarkdown && OnlyMarkdown)
            return ValidationResult.Error(
                "\nOptions:\n\t--nomarkdown \nand\n\t--onlymarkdown\ncannot be used together."
            );

        // Mutually exclusive PDF options
        return NoPdf && OnlyPdf
            ? ValidationResult.Error(
                "\nOptions:\n\t--nopdf \nand\n\t--onlypdf\ncannot be used together."
            )
            : ValidationResult.Success();
    }
}