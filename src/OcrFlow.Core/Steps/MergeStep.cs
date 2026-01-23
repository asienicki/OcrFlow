public sealed class PdfMergeStep : IPipelineStep<PipelineContext>
{
    public void Execute(PipelineContext ctx)
    {
        if (!ctx.Options.MergePdf)
            return;

        var outputPdf = Path.Combine(
            ctx.InputDir,
            ctx.Options.OutputPdfFileName);

        PdfMerger.Merge(ctx.OutputPdfDir, outputPdf);

        Directory.Delete(ctx.OutputPdfDir, true);
    }
}
