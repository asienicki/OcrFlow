using System.Text.RegularExpressions;
using OcrFlow.Markdown.Flow.Abstractions;

namespace OcrFlow.Markdown.Flow.Steps;

public sealed class TextNormalizationStep : ITextStep
{
    public void Execute(TextState state)
    {
        var lines = SplitLines(state.Text).ToList();
        var output = new List<string>(lines.Count);

        foreach (var line in lines)
            output.Add(Normalize(line));

        state.Text = string.Join(Environment.NewLine, output);
    }

    private static string Normalize(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        input = Regex.Replace(input, @"\s+([.,;:!?])", "$1");
        input = Regex.Replace(input, @"([.,;:!?])([A-Za-zĄĆĘŁŃÓŚŻŹąćęłńóśżź])", "$1 $2");
        input = Regex.Replace(input, @"([.,]){2,}", "$1");

        input = Regex.Replace(input, @"""{2,}", @"""");
        input = Regex.Replace(input, @"\s+""", @"""");
        input = Regex.Replace(input, @"""([A-Za-zĄĆĘŁŃÓŚŻŹąćęłńóśżź])", @""" $1");

        input = Regex.Replace(input, @"\s+\)", ")");
        input = Regex.Replace(input, @"\(\s+", "(");

        input = Regex.Replace(input, @"[_]{2,}", "_");
        input = Regex.Replace(input, @"[ \t]{2,}", " ");

        return input.Trim();
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