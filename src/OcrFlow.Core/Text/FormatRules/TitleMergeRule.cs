using System.Text;

public sealed class TitleMergeRule : IFormatRule
{
    public bool CanApply(FormatterContext ctx)
        => IsAllCaps(ctx.Current) && ctx.HasNext && IsAllCaps(ctx.Next);

    public void Apply(FormatterContext ctx)
    {
        var sb = new StringBuilder(ctx.Current);

        while (ctx.HasNext && IsAllCaps(ctx.Next))
        {
            ctx.Advance();
            sb.Append(" ").Append(ctx.Current);
        }

        ctx.Output.AppendLine($"## {sb.ToString().Replace("|", "").Trim()}");
        ctx.Output.AppendLine();
        ctx.Advance();
    }

    private static bool IsAllCaps(string line) =>
        line.Any(char.IsLetter) && line == line.ToUpperInvariant();
}
