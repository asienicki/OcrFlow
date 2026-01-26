using OcrFlow.Markdown.Flow.Abstractions;

namespace OcrFlow.Markdown.Flow.Steps
{
    public sealed class HeaderStep : ITextStep
    {
        private static readonly string[] ForbiddenEndings =
            { " Z", " I", " DLA", " ORAZ", " W ZAKRESIE" };

        public void Execute(TextState state)
        {
            var lines = SplitLines(state.Text);
            var output = new List<string>();

            foreach (var line in lines)
            {
                if (IsHeader(line))
                {
                    output.Add($"## {line}");
                    output.Add(string.Empty); // pusta linia jak w Apply()
                }
                else
                {
                    output.Add(line);
                }
            }

            state.Text = string.Join(Environment.NewLine, output);
        }

        private static bool IsHeader(string line)
        {
            if (!IsAllCaps(line)) return false;
            if (line.Length > 60) return false;
            if (line.Contains("|")) return false;

            if (ForbiddenEndings.Any(e =>
                line.EndsWith(e, StringComparison.OrdinalIgnoreCase)))
                return false;

            if (LooksLikeIdentifier(line)) return false;

            return true;
        }

        private static bool IsAllCaps(string line) =>
            line.Any(char.IsLetter) && line == line.ToUpperInvariant();

        private static bool LooksLikeIdentifier(string line) =>
            line.Contains("ISBN", StringComparison.OrdinalIgnoreCase) ||
            line.Contains("MAZ/", StringComparison.OrdinalIgnoreCase);

        private static IEnumerable<string> SplitLines(string text)
            => text.Replace("\r\n", "\n")
                   .Replace("\r", "\n")
                   .Split('\n')
                   .Select(l => l.Trim())
                   .Where(l => l.Length > 0);
    }
}
