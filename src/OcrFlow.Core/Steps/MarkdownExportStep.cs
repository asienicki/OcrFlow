using System.Text;

public sealed class MarkdownExportStep : IPipelineStep<PipelineContext>
{
    private readonly TextProcessingPipeline _textPipeline;

    public MarkdownExportStep(TextProcessingPipeline textPipeline)
    {
        _textPipeline = textPipeline;
    }

    public void Execute(PipelineContext ctx)
    {
        if (!ctx.Options.ExportMarkdown)
            return;

        var sb = new StringBuilder();
        int pageNo = 1;

        foreach (var rawText in ctx.PageTexts)
        {
            var processed = _textPipeline.Process(rawText);

            sb.AppendLine(string.Format(
                ctx.Options.PageHeaderFormat, pageNo));
            sb.AppendLine();
            sb.AppendLine(MarkdownFormatter.Format(processed));
            sb.AppendLine(ctx.Options.PageSeparator);

            pageNo++;
        }

        var mdPath = Path.Combine(
            ctx.InputDir,
            ctx.Options.OutputMarkdownFileName);

        File.WriteAllText(mdPath, sb.ToString());
    }
}