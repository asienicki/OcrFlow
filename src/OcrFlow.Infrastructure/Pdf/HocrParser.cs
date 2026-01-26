using System.Globalization;
using System.Text.RegularExpressions;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace OcrFlow.Infrastructure.Pdf;

public static class HocrParser
{
    private static readonly Regex LineRegex =
        new(
            @"<span[^>]*class='ocr_line'[^>]*title='[^']*bbox (\d+) (\d+) (\d+) (\d+); baseline ([^']*)'[^>]*>(.*?)</span>",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline
        );

    public static void DrawTextLayer(
        XGraphics gfx,
        string hocr,
        PdfPage page,
        double dpiX,
        double dpiY)
    {
        var scaleX = 72.0 / dpiX;
        var scaleY = 72.0 / dpiY;

        var pageHeightPt = page.Height.Point;

        foreach (Match line in LineRegex.Matches(hocr))
        {
            var x1 = double.Parse(line.Groups[1].Value, CultureInfo.InvariantCulture) * scaleX;
            var y1 = double.Parse(line.Groups[2].Value, CultureInfo.InvariantCulture) * scaleY;
            _ = double.Parse(line.Groups[3].Value, CultureInfo.InvariantCulture) * scaleX;
            var y2 = double.Parse(line.Groups[4].Value, CultureInfo.InvariantCulture) * scaleY;

            var height = y2 - y1;
            var fontSize = height * 0.85;

            var text = Regex
                .Replace(line.Groups[6].Value, "<[^>]+>", " ")
                .Trim();

            if (string.IsNullOrWhiteSpace(text))
                continue;

            var font = new XFont(
                "DejaVuSans",
                fontSize,
                XFontStyleEx.Regular,
                new XPdfFontOptions(
                    PdfFontEncoding.Unicode,
                    PdfFontEmbedding.EmbedCompleteFontFile
                )
            );

            var pdfX = x1;
            var pdfY = pageHeightPt - y2;

            gfx.DrawString(
                text,
                font,
                XBrushes.Black,
                new XPoint(pdfX, pdfY),
                XStringFormats.TopLeft
            );
        }
    }
}