using Microsoft.Extensions.Options;
using OcrFlow.Cli.Commands;
using OcrFlow.Core.Flow.Models.Options;
using OcrFlow.Core.Flow.Models.Runtime;

namespace OcrFlow.Cli.Bootstrap;

public sealed class OcrRunOptionsBuilder
{
    private readonly OcrFlowOptions _config;

    public OcrRunOptionsBuilder(IOptions<OcrFlowOptions> config)
    {
        _config = config.Value;
    }

    public OcrRunOptions Build(OcrCommandSettings cli)
    {
        return new OcrRunOptions
        {
            InputDir = cli.InputDir,
            Merge = !cli.NoMerge,

            GeneratePdf = !cli.OnlyMarkdown && !cli.NoPdf,
            GenerateMarkdown = !cli.OnlyPdf && !cli.NoMarkdown,

            Languages = !string.IsNullOrWhiteSpace(cli.Lang)
                ? cli.Lang.Split(',').Select(x => x.Trim()).ToArray()
                : _config.Tesseract.DefaultLanguages,

            OutputDirectory = cli.OutputDir ?? cli.InputDir,
            MarkdownFileNamePrefix = cli.MarkdownFileNamePrefix ?? "",
            PdfFileNamePrefix = cli.PdfFileNamePrefix ?? ""
        };
    }
}