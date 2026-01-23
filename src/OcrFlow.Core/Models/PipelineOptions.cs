public sealed class PipelineOptions
{
    public bool MergePdf { get; init; } = true;
    public bool ExportMarkdown { get; init; } = true;

    public string OutputPdfFileName { get; init; } = "all.pdf";

    public string OutputMarkdownFileName { get; init; } = "all.md";

    public string PageHeaderFormat { get; init; } = "# Strona {0}";
    public string PageSeparator { get; init; } = "\n---\n";
}
