using Tesseract;

public sealed class OcrSettings
{
    public IReadOnlyList<string> ImageSearchPatterns { get; init; }
    = new[] { "*.png", "*.jpg", "*.jpeg" };
    public string OutputPdfDirName { get; init; } = "pdf";

    public string TessDataDir { get; init; } = "tessdata";
    public string OcrLanguage { get; init; } = "pol";
    public EngineMode EngineMode { get; init; } = EngineMode.LstmOnly;

    public string DictionaryPath { get; init; }
        = Path.Combine("dict", "dict.txt");
}
