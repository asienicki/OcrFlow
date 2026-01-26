using OcrFlow.Markdown.Flow.Abstractions;
using OcrFlow.Markdown.Flow.Steps;

namespace OcrFlow.Markdown.Flow;

public sealed class MarkdownFlow : IMarkdownFlow
{
    private readonly List<ITextStep> _steps = [];

    private MarkdownFlow()
    {
    }

    public IMarkdownFlow RomanNumerals()
    {
        return Add(new RomanNumeralStep());
    }

    public IMarkdownFlow TextNormalization()
    {
        return Add(new TextNormalizationStep());
    }

    public IMarkdownFlow SpellCorrection(string dictPath)
    {
        return Add(new SpellCorrectionStep(dictPath));
    }

    public IMarkdownFlow GarbageRemove()
    {
        return Add(new GarbageRemoveStep());
    }

    public IMarkdownFlow TitleMerge()
    {
        return Add(new TitleMergeStep());
    }

    public IMarkdownFlow SectionLabels()
    {
        return Add(new SectionLabelStep());
    }

    public IMarkdownFlow JoinLines()
    {
        return Add(new JoinLineStep());
    }

    public IMarkdownFlow Identifiers()
    {
        return Add(new IdentifierStep());
    }

    public IMarkdownFlow Headers()
    {
        return Add(new HeaderStep());
    }

    public IMarkdownFlow PlainText()
    {
        return Add(new PlainTextStep());
    }

    public IMarkdownFlow CommaSpacing()
    {
        return Add(new CommaSpacingStep());
    }

    public TextProcess Build()
    {
        return new TextProcess(_steps);
    }

    public static MarkdownFlow Start()
    {
        return new MarkdownFlow();
    }

    private IMarkdownFlow Add(ITextStep step)
    {
        _steps.Add(step);
        return this;
    }
}