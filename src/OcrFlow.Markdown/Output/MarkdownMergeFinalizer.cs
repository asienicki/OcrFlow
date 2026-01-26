using OcrFlow.Core;
using OcrFlow.Core.Flow.Models.Runtime;
using OcrFlow.Core.Output.Abstractions;
using OcrFlow.Infrastructure;

namespace OcrFlow.Markdown.Output;

public sealed class MarkdownMergeFinalizer : IOutputFinalizer
{
    private readonly OcrRunContext _context;

    public MarkdownMergeFinalizer(OcrRunContext context)
    {
        _context = context;
    }

    public bool ShouldRun()
    {
        return _context.Options.Merge;
    }

    public async Task FinalizeAsync(CancellationToken ct)
    {
        var mdDir = PathBuilder.GetMdFolderPath(_context.Options);

        if (!Directory.Exists(mdDir))
            return;

        var files = Directory
            .GetFiles(mdDir, "*.md")
            .OrderBy(f => f, new SemiNumericComparer())
            .ToList();

        if (files.Count == 0)
            return;

        var outputFile = Path.Combine(
            _context.Options.OutputDirectory,
            "all.md");

        await using var writer = new StreamWriter(outputFile);

        foreach (var file in files)
        {
            ct.ThrowIfCancellationRequested();

            var text = await File.ReadAllTextAsync(file, ct);

            await writer.WriteLineAsync(text);
            await writer.WriteLineAsync();
        }
    }
}