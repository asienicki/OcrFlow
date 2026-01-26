using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace OcrFlow.Infrastructure.Pdf;

public static class PdfMerger
{
    public static void Merge(string inputPdfDir, string outputFile)
    {
        var files = Directory.GetFiles(inputPdfDir, "*.pdf")
            .Where(f => !Path.GetFileName(f).Equals(
                Path.GetFileName(outputFile),
                StringComparison.OrdinalIgnoreCase))
            .OrderBy(f => f, new SemiNumericComparer())
            .ToList();

        if (files.Count == 0)
        {
            Console.WriteLine("[WARN] no PDFs to merge");
            return;
        }

        using var target = new PdfDocument();

        foreach (var file in files)
        {
            using var src = PdfReader.Open(file, PdfDocumentOpenMode.Import);
            for (int i = 0; i < src.PageCount; i++)
                target.AddPage(src.Pages[i]);
        }

        target.Save(outputFile);

        Console.WriteLine($"[OK] merged {files.Count} files -> {outputFile}");
    }
}
