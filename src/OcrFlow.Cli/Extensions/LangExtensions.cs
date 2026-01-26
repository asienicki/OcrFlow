using Spectre.Console;

namespace OcrFlow.Cli.Extensions;

public static class LangExtensions
{
    public static IReadOnlyList<string>? ResolveLanguages(this string? lang)
    {
        if (string.IsNullOrWhiteSpace(lang))
            return null; // auto z configu

        if (lang == "*")
            return new[] { "*" };

        return lang
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Trim())
            .ToArray();
    }
}
