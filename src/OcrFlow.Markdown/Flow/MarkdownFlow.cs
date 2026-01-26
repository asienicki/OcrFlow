using OcrFlow.Core.Flow.Abstractions;
using OcrFlow.Markdown.Flow.Abstractions;
using OcrFlow.Markdown.Flow.Steps;

namespace OcrFlow.Markdown.Flow
{

    public sealed class MarkdownFlow : IMarkdownFlow
    {
        private readonly List<ITextStep> _steps = [];

        private MarkdownFlow() { }

        public static MarkdownFlow Start() => new();

        public IMarkdownFlow RomanNumerals()
            => Add(new RomanNumeralStep());

        public IMarkdownFlow TextNormalization()
            => Add(new TextNormalizationStep());

        public IMarkdownFlow SpellCorrection(string dictPath)
            => Add(new SpellCorrectionStep(dictPath));

        public IMarkdownFlow GarbageRemove()
            => Add(new GarbageRemoveStep());

        public IMarkdownFlow TitleMerge()
            => Add(new TitleMergeStep());

        public IMarkdownFlow SectionLabels()
            => Add(new SectionLabelStep());

        public IMarkdownFlow JoinLines()
            => Add(new JoinLineStep());
        public IMarkdownFlow Identifiers()
            => Add(new IdentifierStep());

        public IMarkdownFlow Headers()
            => Add(new HeaderStep());

        public IMarkdownFlow PlainText()
            => Add(new PlainTextStep());

        public IMarkdownFlow CommaSpacing()
            => Add(new CommaSpacingStep());

        private IMarkdownFlow Add(ITextStep step)
        {
            _steps.Add(step);
            return this;
        }

        public TextProcess Build()
            => new(_steps);
    }
}
