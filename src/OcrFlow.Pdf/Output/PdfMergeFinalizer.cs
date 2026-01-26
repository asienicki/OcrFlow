using OcrFlow.Core;
using OcrFlow.Core.Output.Abstractions;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace OcrFlow.Pdf.Output
{
    public sealed class PdfMergeFinalizer : IOutputFinalizer
    {
        private readonly OcrRunContext _context;

        public bool ShouldRun()
            => _context.Options.Merge && _context.Options.GeneratePdf;

        public PdfMergeFinalizer(OcrRunContext context)
        {
            _context = context;
        }

        public Task FinalizeAsync(CancellationToken ct)
        {
            var files = Directory.GetFiles(PathBuilder.GetPdfFolderPath(_context.Options), "*.pdf")
                .OrderBy(f => f, new SemiNumericComparer())
                .ToList();

            if (files.Count == 0)
                return Task.CompletedTask;

            using var target = new PdfDocument();

            foreach (var file in files)
            {
                using var src = PdfReader.Open(file, PdfDocumentOpenMode.Import);
                for (int i = 0; i < src.PageCount; i++)
                    target.AddPage(src.Pages[i]);
            }

            target.Save(Path.Combine(_context.Options.OutputDirectory, "all.pdf"));

            return Task.CompletedTask;
        }
    }
}
