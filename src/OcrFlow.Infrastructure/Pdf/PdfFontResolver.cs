using PdfSharp.Fonts;
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
        bool isBold,
        bool isItalic)
    {
        return new FontResolverInfo("DejaVuSans");
    }
}