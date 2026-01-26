using OcrFlow.Markdown.Flow.Abstractions;
using System.Text.RegularExpressions;

namespace OcrFlow.Markdown.Flow.Steps
{
    public sealed class IdentifierStep : ITextStep
    {
        private static readonly Regex Identifier =
            new(@"\b(ISBN|MAZ/|[0-9]{2,}[-/][0-9])",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public void Execute(TextState state)
        {
            var lines = SplitLines(state.Text);
            var output = new List<string>();

            foreach (var line in lines)
            {
                if (Identifier.IsMatch(line))
                    output.Add($"**{line}**");
                else
                    output.Add(line);
            }

            state.Text = string.Join(Environment.NewLine, output);
        }

        private static IEnumerable<string> SplitLines(string text)
            => text.Replace("\r\n", "\n")
                   .Replace("\r", "\n")
                   .Split('\n')
                   .Select(l => l.Trim())
                   .Where(l => l.Length > 0);
    }
}
