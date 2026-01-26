using OcrFlow.Markdown.Flow.Abstractions;

namespace OcrFlow.Markdown.Flow.Steps
{
    public sealed class SectionLabelStep : ITextStep
    {
        private static readonly string[] Labels =
            { "TEMAT:", "JEDNOSTKA PROJEKTOWA:", "PROJEKTANT:" };

        public void Execute(TextState state)
        {
            var lines = SplitLines(state.Text);
            var output = new List<string>();

            foreach (var line in lines)
            {
                if (IsSectionLabel(line))
                {
                    output.Add($"### {line.TrimEnd(':')}");
                    output.Add(string.Empty);
                }
                else
                {
                    output.Add(line);
                }
            }

            state.Text = string.Join(Environment.NewLine, output);
        }

        private static bool IsSectionLabel(string line)
            => Labels.Any(l => line.Equals(l, StringComparison.OrdinalIgnoreCase));

        private static IEnumerable<string> SplitLines(string text)
            => text.Replace("\r\n", "\n")
                   .Replace("\r", "\n")
                   .Split('\n')
                   .Select(l => l.Trim())
                   .Where(l => l.Length > 0);
    }
}
