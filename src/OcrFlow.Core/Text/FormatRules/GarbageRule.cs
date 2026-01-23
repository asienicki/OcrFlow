using System.Text.RegularExpressions;

public sealed class GarbageRule : IFormatRule
{
    public bool CanApply(FormatterContext ctx)
        => IsGarbage(ctx.Current);

    public void Apply(FormatterContext ctx)
    {
        ctx.Advance(); // skip
    }

    private static bool IsGarbage(string line)
    {
        if (line.Length < 4) return true;
        if (Regex.IsMatch(line, @"\b(ppro|specja|budowla|ości)\b",
            RegexOptions.IgnoreCase)) return true;
        if (Regex.IsMatch(line, @"^['""„”]")) return true;
        return false;
    }
}
