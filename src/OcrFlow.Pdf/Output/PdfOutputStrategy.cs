using OcrFlow.Core;
using OcrFlow.Core.Flow.Models;
using OcrFlow.Core.Output.Abstractions;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace OcrFlow.Pdf.Output
{

    public sealed class PdfOutputStrategy : IOcrOutputStrategy
    {
        private readonly OcrRunContext _context;

        public bool ShouldRun()
            => _context.Options.GeneratePdf;

        public PdfOutputStrategy(OcrRunContext context)
        {
            _context = context;
        }

        public Task RunAsync(OcrOutput output, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            BuildPdf(
                output.SourceImagePath,
                output.HocrText,
                PathBuilder.GetPdfPath(_context.Options, output.SourceImagePath)
            );

            return Task.CompletedTask;
        }

        private static void BuildPdf(
            string imagePath,
            string hocr,
            string outputPdf)
        {
            using var doc = new PdfDocument();
            var page = doc.AddPage();

            using var img = XImage.FromFile(imagePath);

            // px -> pt (72 pt = 1 inch)
            var widthPt = img.PixelWidth * 72.0 / img.HorizontalResolution;
            var heightPt = img.PixelHeight * 72.0 / img.VerticalResolution;

            page.Width = XUnit.FromPoint(widthPt);
            page.Height = XUnit.FromPoint(heightPt);

            using var gfx = XGraphics.FromPdfPage(page);

            // obraz
            gfx.DrawImage(img, 0, 0, page.Width.Point, page.Height.Point);

            // warstwa OCR
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
}
