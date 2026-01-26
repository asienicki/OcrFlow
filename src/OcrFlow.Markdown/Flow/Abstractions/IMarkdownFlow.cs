namespace OcrFlow.Markdown.Flow.Abstractions;

public interface IMarkdownFlow
{
    IMarkdownFlow RomanNumerals();

    IMarkdownFlow TextNormalization();

    IMarkdownFlow SpellCorrection(string dictPath);

    IMarkdownFlow GarbageRemove();

    IMarkdownFlow TitleMerge();

    IMarkdownFlow SectionLabels();

    IMarkdownFlow JoinLines();

    IMarkdownFlow Identifiers();

    IMarkdownFlow Headers();

    IMarkdownFlow PlainText();

    IMarkdownFlow CommaSpacing();

    TextProcess Build();
}