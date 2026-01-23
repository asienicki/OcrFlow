public sealed class CollectTextStep : IPipelineStep<PageContext>
{
    private readonly PipelineContext _pipeline;

    public CollectTextStep(PipelineContext pipeline)
    {
        _pipeline = pipeline;
    }

    public void Execute(PageContext ctx)
    {
        _pipeline.PageTexts.Add(ctx.RawText);
    }
}
