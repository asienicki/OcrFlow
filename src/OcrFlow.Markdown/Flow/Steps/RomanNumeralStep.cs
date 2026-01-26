using OcrFlow.Markdown.Flow.Abstractions;
using System.Text.RegularExpressions;

namespace OcrFlow.Markdown.Flow.Steps
{
    public sealed class RomanNumeralStep : ITextStep
    {
        // Początek linii: I + (l lub 1) + kropka + spacja + WIELKA LITERA
        private static readonly Regex RomanStart =
            new(@"^(I[l1]{1,3})\.\s+(?=[A-ZĄĆĘŁŃÓŚŻŹ])",
                RegexOptions.Compiled);

        public void Execute(TextState state)
        {
            var lines = SplitLines(state.Text);
            var output = new List<string>();

            foreach (var line in lines)
                output.Add(ProcessLine(line));

            state.Text = string.Join(Environment.NewLine, output);
        }

        private static string ProcessLine(string line)
        {
            var m = RomanStart.Match(line);
            if (!m.Success)
                return line;

            var raw = m.Groups[1].Value;

            // normalizacja: l / 1 → I
            var fixedRoman = raw
                .Replace('l', 'I')
                .Replace('1', 'I');

            return fixedRoman + line.Substring(raw.Length);
        }

        private static IEnumerable<string> SplitLines(string text)
            => text.Replace("\r\n", "\n")
                   .Replace("\r", "\n")
                   .Split('\n')
                   .Select(l => l.Trim())
                   .Where(l => l.Length > 0);
    }
}