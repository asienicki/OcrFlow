namespace OcrFlow.Core.Flow.Abstractions
{
    public interface IOcrStepFactory
    {
        T Create<T>() where T : IOcrStep;
    }
}
