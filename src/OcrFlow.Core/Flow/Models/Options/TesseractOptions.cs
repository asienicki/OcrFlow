namespace OcrFlow.Core.Flow.Models.Options;

public sealed class TesseractOptions
{
    public IReadOnlyList<TessDataSource> DataSources { get; init; } = [];

    public IReadOnlyList<string> DefaultLanguages { get; init; } = [];
}