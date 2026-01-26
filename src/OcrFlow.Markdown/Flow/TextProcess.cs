using OcrFlow.Markdown.Flow.Abstractions;

namespace OcrFlow.Markdown.Flow;

public sealed class TextProcess
{
    private readonly IReadOnlyList<ITextStep> _steps;

    public TextProcess(IEnumerable<ITextStep> steps)
    {
        _steps = steps.ToList();
    }

    public string Execute(string input)
    {
        var state = new TextState(input);

        foreach (var step in _steps)
            step.Execute(state);

        return state.Text;
    }
}