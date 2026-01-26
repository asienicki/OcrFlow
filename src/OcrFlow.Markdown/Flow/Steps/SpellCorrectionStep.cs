using System.Text.RegularExpressions;
using OcrFlow.Markdown.Flow.Abstractions;
using static SymSpell;

namespace OcrFlow.Markdown.Flow.Steps;

public sealed class SpellCorrectionStep : ITextStep
{
    private static readonly Regex Word =
        new(@"\b[a-ząćęłńóśżź]{9,}\b",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private readonly HashSet<string> _dictionary;
    private readonly SymSpell _sym;

    public SpellCorrectionStep(string dictionaryPath)
    {
        _sym = new SymSpell(maxDictionaryEditDistance: 2, prefixLength: 7);
        _ = _sym.LoadDictionary(dictionaryPath, 0, 1);

        _dictionary = File.ReadLines(dictionaryPath)
            .Select(l => l.Split(' ')[0])
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    public void Execute(TextState state)
    {
        var lines = SplitLines(state.Text).ToList();
        var output = new List<string>(lines.Count);

        foreach (var line in lines)
            output.Add(Correct(line));

        state.Text = string.Join(Environment.NewLine, output);
    }

    private string Correct(string input)
    {
        return Word.Replace(input, m =>
        {
            var w = m.Value;

            if (char.IsUpper(w[0]))
                return w;

            if (_dictionary.Contains(w))
                return w;

            var suggestions = _sym.Lookup(w, Verbosity.Top, 1);
            if (suggestions.Count != 1)
                return w;

            var best = suggestions[0].term;

            return HasSimilarEnding(w, best) ? best : w;
        });
    }

    private static bool HasSimilarEnding(string a, string b)
    {
        return a.Length < 3 || b.Length < 3 ? false : a[^3..].Equals(b[^3..], StringComparison.OrdinalIgnoreCase);
    }

    private static IEnumerable<string> SplitLines(string text)
    {
        return text.Replace("\r\n", "\n")
            .Replace("\r", "\n")
            .Split('\n')
            .Select(l => l.Trim())
            .Where(l => l.Length > 0);
    }
}