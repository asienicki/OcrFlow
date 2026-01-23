public interface IFormatRule
{
    bool CanApply(FormatterContext ctx);
    void Apply(FormatterContext ctx);
}
