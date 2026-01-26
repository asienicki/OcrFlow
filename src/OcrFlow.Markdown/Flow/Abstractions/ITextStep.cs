namespace OcrFlow.Markdown.Flow.Abstractions;

public interface ITextStep
{
    void Execute(TextState state);
}