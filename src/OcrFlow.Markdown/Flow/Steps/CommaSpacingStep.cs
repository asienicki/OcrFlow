using System.Text.RegularExpressions;
using OcrFlow.Markdown.Flow.Abstractions;

namespace OcrFlow.Markdown.Flow.Steps;

public sealed class CommaSpacingStep : ITextStep
{
    public void Execute(TextState state)
    {
        state.Text = NormalizeCommas(state.Text);
    }

    private static string NormalizeCommas(string text)
    {
        // " , " -> ", "
        text = text.Replace(" , ", ", ");

        // brak spacji po przecinku przed literą: ",A" / ",a" -> ", A"
        text = Regex.Replace(
            text,
            ",(?=[A-Za-z])",
            ", "
        );

        return text;
    }
}