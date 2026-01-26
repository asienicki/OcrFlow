using System.Text;

namespace OcrFlow.Cli.Ui;

public static class TableMarkdownExporter
{
    public static void Save(string path, IEnumerable<PageStatus> pages)
    {
        var list = pages.OrderBy(x => x.PageNo).ToList();
        var (count, avgMs) = ComputeSummary(list);

        var sb = new StringBuilder()
         .AppendLine("## OCR result")
         .AppendLine()
         .AppendLine($"- Pages: **{count}**")
         .AppendLine($"- Avg OCR time: **{avgMs:F1} ms**")
         .AppendLine()
         .AppendLine("| # | File | Status | OCR ms |")
         .AppendLine("|---|------|--------|--------|");

        foreach (var p in list)
        {
            _ = sb.Append("| ")
              .Append(p.PageNo).Append(" | ")
              .Append(p.File).Append(" | ")
              .Append(Clean(p.Status)).Append(" | ")
              .Append(p.OcrMs == 0 ? "-" : p.OcrMs).AppendLine(" |");
        }

        File.WriteAllText(path, sb.ToString());
    }
    private static (int pages, double avgMs) ComputeSummary(IEnumerable<PageStatus> pages)
    {
        var done = pages.Where(p => p.OcrMs > 0).ToList();

        var count = done.Count;
        var avg = count == 0 ? 0 : done.Average(p => p.OcrMs);

        return (count, avg);
    }

    private static string Clean(string status)
        => status
            .Replace("[green]", "")
            .Replace("[/]", "")
            .Replace("[yellow]", "")
            .Replace("[red]", "");
}