using System.Text.RegularExpressions;
using static SymSpell;

public sealed class SpellCorrectionProcessor : ITextProcessor
{
    private readonly SymSpell _sym;
    private readonly HashSet<string> _dictionary;

    private static readonly Regex Word =
        new(@"\b[a-ząćęłńóśżź]{9,}\b", RegexOptions.Compiled);

    public SpellCorrectionProcessor(string dictionaryPath)
    {
        _sym = new SymSpell(maxDictionaryEditDistance: 2, prefixLength: 7);
        _sym.LoadDictionary(dictionaryPath, termIndex: 0, countIndex: 1);

        _dictionary = File.ReadLines(dictionaryPath)
            .Select(l => l.Split(' ')[0])
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    public string Process(string input)
    {
        return Word.Replace(input, m =>
        {
            var w = m.Value;

            if (char.IsUpper(w[0]))
                return w;

            if (_dictionary.Contains(w))
                return w;

            var suggestions = _sym.Lookup(w, Verbosity.Top, maxEditDistance: 1);

            if (suggestions.Count != 1)
                return w;

            var best = suggestions[0].term;

            if (!HasSimilarEnding(w, best))
                return w;

            return best;
        });
    }

    private static bool HasSimilarEnding(string a, string b)
    {
        if (a.Length < 3 || b.Length < 3)
            return false;

        return a[^3..].Equals(b[^3..], StringComparison.OrdinalIgnoreCase);
    }
}