public sealed class PdfBuildStep : IPipelineStep<PageContext>
{
    public void Execute(PageContext ctx)
    {
        var pdfPath = Path.Combine(
            ctx.OutputPdfDir,
            $"{Path.GetFileNameWithoutExtension(ctx.ImagePath)}.pdf");

        PdfBuilder.Build(ctx.ImagePath, ctx.Hocr, pdfPath);
    }
}
