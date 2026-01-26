using OcrFlow.Markdown.Flow.Abstractions;
using System.Text;

namespace OcrFlow.Markdown.Flow.Steps
{
    public sealed class TitleMergeStep : ITextStep
    {
        public void Execute(TextState state)
        {
            var lines = SplitLines(state.Text).ToList();
            var output = new List<string>();

            for (int i = 0; i < lines.Count; i++)
            {
                var current = lines[i];

                if (IsAllCaps(current) && i + 1 < lines.Count && IsAllCaps(lines[i + 1]))
                {
                    var sb = new StringBuilder(current);

                    while (i + 1 < lines.Count && IsAllCaps(lines[i + 1]))
                    {
                        i++;
                        _ = sb.Append(' ').Append(lines[i]);
                    }

                    output.Add($"## {sb.ToString().Replace("|", "").Trim()}");
                    output.Add(string.Empty);
                }
                else
                {
                    output.Add(current);
                }
            }

            state.Text = string.Join(Environment.NewLine, output);
        }

        private static bool IsAllCaps(string line) =>
            line.Any(char.IsLetter) && line == line.ToUpperInvariant();

        private static IEnumerable<string> SplitLines(string text)
            => text.Replace("\r\n", "\n")
                   .Replace("\r", "\n")
                   .Split('\n')
                   .Select(l => l.Trim())
                   .Where(l => l.Length > 0);
    }
}
