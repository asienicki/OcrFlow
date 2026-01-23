public interface IPipelineStep<TContext>
{
    void Execute(TContext context);
}

public interface IProgressReporter
{
    void PageStarted(int pageNo, string file);
    void PageCompleted(int pageNo, long ocrMs);
}