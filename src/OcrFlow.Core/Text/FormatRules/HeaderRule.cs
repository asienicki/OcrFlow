public sealed class HeaderRule : IFormatRule
{
    private static readonly string[] ForbiddenEndings =
        { " Z", " I", " DLA", " ORAZ", " W ZAKRESIE" };

    public bool CanApply(FormatterContext ctx)
    {
        var line = ctx.Current;

        if (!IsAllCaps(line)) return false;
        if (line.Length > 60) return false;
        if (line.Contains("|")) return false;

        if (ForbiddenEndings.Any(e =>
            line.EndsWith(e, StringComparison.OrdinalIgnoreCase)))
            return false;

        if (LooksLikeIdentifier(line)) return false;

        return true;
    }

    public void Apply(FormatterContext ctx)
    {
        ctx.Output.AppendLine($"## {ctx.Current}");
        ctx.Output.AppendLine();
        ctx.Advance();
    }

    private static bool IsAllCaps(string line) =>
        line.Any(char.IsLetter) && line == line.ToUpperInvariant();

    private static bool LooksLikeIdentifier(string line) =>
        line.Contains("ISBN", StringComparison.OrdinalIgnoreCase) ||
        line.Contains("MAZ/");
}