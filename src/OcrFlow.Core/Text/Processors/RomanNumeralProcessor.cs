using System.Text.RegularExpressions;

public class RomanNumeralProcessor : ITextProcessor
{
    // Początek linii: I + (l lub 1) + kropka + spacja + WIELKA LITERA
    private static readonly Regex RomanStart =
        new(@"^(I[l1]{1,3})\.\s+(?=[A-ZĄĆĘŁŃÓŚŻŹ])",
            RegexOptions.Compiled);

    public string Process(string input)
    {
        var m = RomanStart.Match(input);
        if (!m.Success)
            return input;

        var raw = m.Groups[1].Value;

        // normalizacja: l / 1 → I
        var fixedRoman = raw.Replace('l', 'I').Replace('1', 'I');

        return fixedRoman + input.Substring(raw.Length);
    }
}
