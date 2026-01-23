using System.Text.RegularExpressions;

public static class MarkdownFormatter
{
    private static readonly IFormatRule[] Rules =
    {
        new GarbageRule(),
        new TitleMergeRule(),
        new SectionLabelRule(),
        new JoinLineRule(),
        new IdentifierRule(),
        new HeaderRule(),
        new PlainTextRule()
    };

    public static string Format(string rawText)
    {
        if (string.IsNullOrWhiteSpace(rawText))
            return string.Empty;

        var lines = rawText
            .Replace("\r\n", "\n")
            .Replace("\r", "\n")
            .Split('\n')
            .Select(l => Regex.Replace(l.Trim(), @"\s{2,}", " "))
            .Where(l => l.Length > 0)
            .ToList();

        var ctx = new FormatterContext(lines);

        while (ctx.Index < ctx.Lines.Count)
        {
            foreach (var rule in Rules)
            {
                if (rule.CanApply(ctx))
                {
                    rule.Apply(ctx);
                    break;
                }
            }
        }

        return ctx.Output.ToString().Trim();
    }
}