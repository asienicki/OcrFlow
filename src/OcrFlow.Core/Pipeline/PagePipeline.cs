public sealed class PagePipeline
{
    private readonly IPipelineStep<PageContext>[] _steps;

    public PagePipeline(params IPipelineStep<PageContext>[] steps)
    {
        _steps = steps;
    }

    public void Run(PageContext ctx)
    {
        foreach (var step in _steps)
        {
            step.Execute(ctx);
        }
    }
}
