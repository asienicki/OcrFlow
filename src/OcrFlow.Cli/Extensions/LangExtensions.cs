namespace OcrFlow.Cli.Extensions;

public static class LangExtensions
{
    public static IReadOnlyList<string>? ResolveLanguages(this string? lang)
    {
        if (string.IsNullOrWhiteSpace(lang))
            return null; // auto z configu

        return lang == "*"
            ? new[] { "*" }
            : lang
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(l => l.Trim())
                .ToArray();
    }
}