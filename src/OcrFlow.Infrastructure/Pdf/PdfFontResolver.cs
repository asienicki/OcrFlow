using PdfSharp.Fonts;

namespace OcrFlow.Infrastructure.Pdf;

public class PdfFontResolver : IFontResolver
{
    public byte[] GetFont(string faceName)
    {
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "fonts",
            "DejaVuSans.ttf"
        );
        return File.ReadAllBytes(path);
    }

    public FontResolverInfo ResolveTypeface(
        string familyName,
        bool bold,
        bool italic)
    {
        return new FontResolverInfo("DejaVuSans");
    }
}