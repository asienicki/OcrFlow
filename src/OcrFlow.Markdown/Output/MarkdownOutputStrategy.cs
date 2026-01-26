using OcrFlow.Core;
using OcrFlow.Core.Flow.Models;
using OcrFlow.Core.Output.Abstractions;
using OcrFlow.Markdown.Flow;

namespace OcrFlow.Markdown.Output
{
    internal sealed class MarkdownOutputStrategy : IOcrOutputStrategy
    {
        private readonly OcrRunContext _context;

        public bool ShouldRun()
            => _context.Options.GenerateMarkdown;

        public MarkdownOutputStrategy(OcrRunContext context)
        {
            _context = context;
        }

        public async Task RunAsync(OcrOutput output, CancellationToken ct)
        {
            var process = MarkdownFlow
                        .Start()
                        .GarbageRemove()
                        .TitleMerge()
                        .SectionLabels()
                        .JoinLines()
                        .Identifiers()
                        .Headers()
                        .PlainText()
                        .CommaSpacing()
                        .Build();

            if (output.RawText is null)
                throw new InvalidOperationException("output RawText is null");

            var markdown = process.Execute(output.RawText);

            await File.WriteAllTextAsync(PathBuilder.GetMarkdownPath(_context.Options, output.SourceImagePath), markdown, ct);
        }
    }
}
