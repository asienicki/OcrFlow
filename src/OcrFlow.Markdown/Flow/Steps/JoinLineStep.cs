using OcrFlow.Markdown.Flow.Abstractions;

namespace OcrFlow.Markdown.Flow.Steps;

public sealed class JoinLineStep : ITextStep
{
    private static readonly string[] JoinEndings =
        { " Z", " I", " DLA", " ORAZ", " W ZAKRESIE" };

    public void Execute(TextState state)
    {
        var lines = SplitLines(state.Text);
        var output = new List<string>();

        for (var i = 0; i < lines.Count; i++)
        {
            var current = lines[i];

            if (i + 1 < lines.Count &&
                ShouldJoin(current, lines[i + 1]))
            {
                output.Add($"{current} {lines[i + 1]}");
                i++; // skip next
            }
            else
            {
                output.Add(current);
            }
        }

        state.Text = string.Join(Environment.NewLine, output);
    }

    private static bool ShouldJoin(string current, string next)
    {
        return EndsSentence(current)
            ? false
            : !StartsUpper(next)
                ? false
                : JoinEndings.Any(e =>
                    current.EndsWith(e, StringComparison.OrdinalIgnoreCase));
    }

    private static bool EndsSentence(string line)
    {
        return line.EndsWith(".") || line.EndsWith(":") || line.EndsWith(";");
    }

    private static bool StartsUpper(string line)
    {
        return line.Length > 0 && char.IsUpper(line[0]);
    }

    private static List<string> SplitLines(string text)
    {
        return text.Replace("\r\n", "\n")
            .Replace("\r", "\n")
            .Split('\n')
            .Select(l => l.Trim())
            .Where(l => l.Length > 0)
            .ToList();
    }
}