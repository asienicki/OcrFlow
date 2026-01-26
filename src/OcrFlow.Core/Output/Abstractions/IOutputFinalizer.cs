namespace OcrFlow.Core.Output.Abstractions;

public interface IOutputFinalizer
{
    bool ShouldRun();
    Task FinalizeAsync(CancellationToken ct);
}