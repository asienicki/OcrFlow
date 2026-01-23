using System.Text.RegularExpressions;

public class SemiNumericComparer : IComparer<string>
{
    private static readonly Regex Num = new(@"(\d+)", RegexOptions.Compiled);

    public int Compare(string? a, string? b)
    {
        if (a == null || b == null) return 0;

        var na = Num.Match(Path.GetFileNameWithoutExtension(a));
        var nb = Num.Match(Path.GetFileNameWithoutExtension(b));

        if (na.Success && nb.Success &&
            int.TryParse(na.Value, out var ia) &&
            int.TryParse(nb.Value, out var ib))
            return ia.CompareTo(ib);

        return StringComparer.OrdinalIgnoreCase.Compare(a, b);
    }
}
