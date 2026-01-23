using Tesseract;

public sealed class PageContext
{
    public int PageNo { get; init; }
    public string ImagePath { get; init; } = null!;

    public string RawText { get; set; } = "";
    public string Hocr { get; set; } = "";

    public string OutputPdfDir { get; init; } = null!;
    public TesseractEngine Engine { get; init; } = null!;
}