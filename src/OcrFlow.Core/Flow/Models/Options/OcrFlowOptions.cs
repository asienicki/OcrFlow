namespace OcrFlow.Core.Flow.Models.Options;

/// <summary>
///     Loaded from config (appsettings.json) - options for the whole OCR flow
/// </summary>
public sealed class OcrFlowOptions
{
    public RotateOptions Rotate { get; init; } = new();

    public NormalizeDpiOptions NormalizeDpi { get; init; } = new();

    public DeskewOptions Deskew { get; init; } = new();

    public TesseractOptions Tesseract { get; init; } = new();
}