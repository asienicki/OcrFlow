using System.Collections.Concurrent;

public sealed class PipelineContext
{
    public PipelineOptions Options { get; init; } = null!;
    public string InputDir { get; init; } = null!;
    public string OutputPdfDir { get; init; } = null!;

    public ConcurrentBag<string> PageTexts { get; } = new();
}
