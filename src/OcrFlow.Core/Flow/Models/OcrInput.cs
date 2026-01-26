namespace OcrFlow.Core.Flow.Models;

public sealed record OcrInput
{
    public string ImagePath { get; init; } = default!;


    /// <summary>
    ///     null = auto (z configu)
    ///     ["pol"] = wymuś
    ///     ["*"] = wszystkie
    /// </summary>
    public IReadOnlyList<string>? Languages { get; init; }
}