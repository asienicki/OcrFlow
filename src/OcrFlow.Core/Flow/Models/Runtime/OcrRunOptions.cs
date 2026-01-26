public sealed class OcrRunOptions
{
    public string InputDir { get; init; } = null!;

    public bool GeneratePdf { get; init; }

    public bool GenerateMarkdown { get; init; }

    public bool Merge { get; init; }

    public IReadOnlyList<string> Languages { get; init; } = [];

    public string OutputDirectory { get; set; }

    public string? MarkdownFileNamePrefix { get; set; } = "";

    public string? PdfFileNamePrefix { get; set; } = "";

    public IReadOnlySet<string> ImageExtensions { get; init; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        { ".png", ".jpg", ".jpeg" };
}