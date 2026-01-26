using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace OcrFlow.Infrastructure.Pdf;

public static class PdfBuilder
{
    public static void Build(string imagePath, string hocr, string outputPdf)
    {
        using var doc = new PdfDocument();
        var page = doc.AddPage();

        using var img = XImage.FromFile(imagePath);

        // px -> pt (72 pt = 1 inch)
        var pageWidthPt = img.PixelWidth * 72.0 / img.HorizontalResolution;
        var pageHeightPt = img.PixelHeight * 72.0 / img.VerticalResolution;

        page.Width = XUnit.FromPoint(pageWidthPt);
        page.Height = XUnit.FromPoint(pageHeightPt);

        using var gfx = XGraphics.FromPdfPage(page);

        // 1️⃣ OBRAZ
        gfx.DrawImage(
            img,
            0,
            0,
            page.Width.Point,
            page.Height.Point
        );

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
