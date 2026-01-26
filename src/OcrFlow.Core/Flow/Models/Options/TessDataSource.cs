namespace OcrFlow.Core.Flow.Models.Options;

public sealed class TessDataSource
{
    public string Path { get; init; } = default!;
    public IReadOnlyList<string> Languages { get; init; } = [];
}