namespace OcrFlow.Markdown.Flow
{
    public sealed class TextState
    {
        public string Input { get; }
        public string Text { get; set; }

        public TextState(string input)
        {
            Input = input;
            Text = input;
        }
    }
}
