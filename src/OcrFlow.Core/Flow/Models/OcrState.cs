using SixLabors.ImageSharp;

namespace OcrFlow.Core.Flow.Models;

public sealed class OcrState
{
    public OcrState(OcrInput input)
    {
        Input = input;
    }

    public OcrInput Input { get; init; } = default!;

    public OcrOutput? Output { get; set; }

    public Image? Image { get; set; } // obraz w pamięci (ImageSharp)

    public int? Dpi { get; set; } // aktualne DPI
}