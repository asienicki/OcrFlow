using OcrFlow.Core.Flow.Models;

namespace OcrFlow.Core.Flow.Abstractions;

public interface IOcrStep
{
    bool IsEnabled { get; }

    ValueTask ExecuteAsync(OcrState state, CancellationToken ct);
}