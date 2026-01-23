public sealed class JoinLineRule : IFormatRule
{
    private static readonly string[] JoinEndings =
        { " Z", " I", " DLA", " ORAZ", " W ZAKRESIE" };

    public bool CanApply(FormatterContext ctx)
    {
        if (!ctx.HasNext) return false;
        if (EndsSentence(ctx.Current)) return false;
        if (!StartsUpper(ctx.Next)) return false;

        return JoinEndings.Any(e =>
            ctx.Current.EndsWith(e, StringComparison.OrdinalIgnoreCase));
    }

    public void Apply(FormatterContext ctx)
    {
        var joined = $"{ctx.Current} {ctx.Next}";
        ctx.Output.AppendLine(joined);
        ctx.Advance(); // current
        ctx.Advance(); // next
    }

    private static bool EndsSentence(string line) =>
        line.EndsWith(".") || line.EndsWith(":") || line.EndsWith(";");

    private static bool StartsUpper(string line) =>
        line.Length > 0 && char.IsUpper(line[0]);
}
