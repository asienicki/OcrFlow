using System.Diagnostics;

public sealed class TextProcessingPipeline
{
    private readonly IReadOnlyList<ITextProcessor> _processors;

    public TextProcessingPipeline(IEnumerable<ITextProcessor> processors)
    {
        _processors = processors.ToList();
    }

    public TextProcessingPipeline(params ITextProcessor[] processors)
    {
        _processors = processors;
    }

    public string Process(string input)
    {
        var text = input;

        foreach (var processor in _processors)
        {
            text = processor.Process(text);
        }

        return text;
    }
}