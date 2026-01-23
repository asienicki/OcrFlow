using PdfSharp.Drawing;
using PdfSharp.Pdf;

public static class PdfBuilder
{
    public static void Build(string imagePath, string hocr, string outputPdf)
    {
        using var doc = new PdfDocument();
        var page = doc.AddPage();

        using var img = XImage.FromFile(imagePath);

        page.Width = img.PixelWidth * 72 / img.HorizontalResolution;
        page.Height = img.PixelHeight * 72 / img.VerticalResolution;

        using var gfx = XGraphics.FromPdfPage(page);

        // 1️⃣ OBRAZ
        gfx.DrawImage(img, 0, 0, page.Width, page.Height);

        // 2️⃣ TEKST NAD OBRAZEM (OCR layer)
        HocrParser.DrawTextLayer(
            gfx,
            hocr,
            page,
            img.HorizontalResolution,
            img.VerticalResolution
        );

        doc.Save(outputPdf);
    }
}