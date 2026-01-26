using System.Text.RegularExpressions;
using OcrFlow.Markdown.Flow.Abstractions;

namespace OcrFlow.Markdown.Flow.Steps;

public sealed class GarbageRemoveStep : ITextStep
{
    private static readonly Regex GarbageWords =
        new(@"\b(ppro|specja|budowla|ości)\b",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex GarbageStart =
        new(@"^['""„”]",
            RegexOptions.Compiled);

    public void Execute(TextState state)
    {
        var lines = SplitLines(state.Text);

        var filtered = lines
            .Where(l => !IsGarbage(l))
            .ToList();

        state.Text = string.Join(Environment.NewLine, filtered);
    }

    private static bool IsGarbage(string line)
    {
        return line.Length < 4 ? true : GarbageWords.IsMatch(line) ? true : GarbageStart.IsMatch(line);
    }

    private static IEnumerable<string> SplitLines(string text)
    {
        return text.Replace("\r\n", "\n")
            .Replace("\r", "\n")
            .Split('\n')
            .Select(l => l.Trim())
            .Where(l => l.Length > 0);
    }
}