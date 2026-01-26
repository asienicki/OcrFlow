using OcrFlow.Core.Flow.Abstractions;
using OcrFlow.Core.Flow.Models;

namespace OcrFlow.Core.Flow.Steps.Execution;

public sealed class OcrProcess : IOcrStep
{
    private readonly IOcrStep[] _steps;


    public OcrProcess(IEnumerable<IOcrStep> steps)
    {
        _steps = steps as IOcrStep[] ?? steps.ToArray();
    }

    public bool IsEnabled => true;

    public async ValueTask ExecuteAsync(OcrState state, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(state);

        foreach (var step in _steps)
        {
            if (!step.IsEnabled)
                continue;

            await step.ExecuteAsync(state, ct);
        }

        if (state.Output is null)
            throw new InvalidOperationException("OCR process completed without output.");
    }
}