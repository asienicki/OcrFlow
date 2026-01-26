using OcrFlow.Cli.Commands;

namespace OcrFlow.Cli.Extensions;

public static class OcrCommandSettingsExtensions
{
    public static OcrRunOptions Map(this OcrCommandSettings s)
    {
        return new OcrRunOptions
        {
            InputDir = s.InputDir,
            Languages = s.Lang.ResolveLanguages(),
            GenerateMarkdown = !s.NoMarkdown,
            GeneratePdf = !s.NoPdf,
            OutputDirectory = s.OutputDir ?? s.InputDir
        };
    }
}
