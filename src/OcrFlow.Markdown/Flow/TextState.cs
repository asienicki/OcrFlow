namespace OcrFlow.Markdown.Flow;

public sealed class TextState
{
    public TextState(string input)
    {
        Input = input;
        Text = input;
    }

    public string Input { get; }
    public string Text { get; set; }
}