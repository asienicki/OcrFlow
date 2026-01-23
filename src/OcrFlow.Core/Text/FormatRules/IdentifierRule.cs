using System.Text.RegularExpressions;

public sealed class IdentifierRule : IFormatRule
{
    private static readonly Regex Identifier =
        new(@"\b(ISBN|MAZ/|[0-9]{2,}[-/][0-9])",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public bool CanApply(FormatterContext ctx)
        => Identifier.IsMatch(ctx.Current);

    public void Apply(FormatterContext ctx)
    {
        ctx.Output.AppendLine($"**{ctx.Current}**");
        ctx.Advance();
    }
}
