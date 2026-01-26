using OcrFlow.Core.Flow.Models;

namespace OcrFlow.Core.Output.Abstractions;

public interface IOcrOutputStrategy
{
    bool ShouldRun();

    Task RunAsync(
        OcrOutput output,
        CancellationToken ct);
}