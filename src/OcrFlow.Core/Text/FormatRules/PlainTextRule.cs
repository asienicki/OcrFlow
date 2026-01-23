public sealed class PlainTextRule : IFormatRule
{
    public bool CanApply(FormatterContext ctx) => true;

    public void Apply(FormatterContext ctx)
    {
        ctx.Output.AppendLine(ctx.Current);
        ctx.Advance();
    }
}
