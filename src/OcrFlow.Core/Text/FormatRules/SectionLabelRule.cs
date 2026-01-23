public sealed class SectionLabelRule : IFormatRule
{
    private static readonly string[] Labels =
        { "TEMAT:", "JEDNOSTKA PROJEKTOWA:", "PROJEKTANT:" };

    public bool CanApply(FormatterContext ctx)
        => Labels.Any(l => ctx.Current.Equals(l, StringComparison.OrdinalIgnoreCase));

    public void Apply(FormatterContext ctx)
    {
        ctx.Output.AppendLine($"### {ctx.Current.TrimEnd(':')}");
        ctx.Output.AppendLine();
        ctx.Advance();
    }
}
