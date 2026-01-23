using Tesseract;

public sealed class OcrStep : IPipelineStep<PageContext>
{
    public void Execute(PageContext ctx)
    {
        using var pix = Pix.LoadFromFile(ctx.ImagePath);
             
        using var page = ctx.Engine.Process(pix, PageSegMode.SingleColumn);

        ctx.RawText = page.GetText();
        ctx.Hocr = page.GetHOCRText(0);
    }
}
