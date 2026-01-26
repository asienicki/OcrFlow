namespace OcrFlow.Core.Flow.Models.Options;

public sealed class NormalizeDpiOptions
{
    public bool Enabled { get; init; } = true;
    public int TargetDpi { get; init; } = 300;
}