using System.Text;

public sealed class FormatterContext
{
    public IReadOnlyList<string> Lines { get; }
    public int Index { get; set; }
    public StringBuilder Output { get; } = new();

    public string Current => Lines[Index];

    public FormatterContext(IReadOnlyList<string> lines)
    {
        Lines = lines;
    }

    public bool HasNext => Index < Lines.Count - 1;
    public string Next => HasNext ? Lines[Index + 1] : string.Empty;

    public void Advance() => Index++;
}
