using OcrFlow.Markdown.Flow.Abstractions;

namespace OcrFlow.Markdown.Flow.Steps;

public sealed class PlainTextStep : ITextStep
{
    public void Execute(TextState state)
    {
        var lines = SplitLines(state.Text);
        state.Text = string.Join(Environment.NewLine, lines);
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