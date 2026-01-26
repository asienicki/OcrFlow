namespace OcrFlow.Core.Flow.Models;

public class OcrOutput
{
    public string HocrText { get; init; } = default!;

    public string? RawText { get; init; }

    public required string SourceImagePath { get; init; }
}