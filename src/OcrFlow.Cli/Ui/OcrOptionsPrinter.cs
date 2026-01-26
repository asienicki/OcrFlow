using Spectre.Console;

namespace OcrFlow.Cli.Ui;

internal static class OcrOptionsPrinter
{
    public static void Print(OcrRunOptions o)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .Title("[bold]OCR Run Options[/]")
            .AddColumn("Parameter")
            .AddColumn("Value");

        table.AddRow("InputDir", o.InputDir);
        table.AddRow("Languages", string.Join(", ", o.Languages));
        table.AddRow("Extensions", string.Join(", ", o.ImageExtensions));
        table.AddRow("GeneratePdf", o.GeneratePdf.ToString());
        table.AddRow("OnlyMarkdown", o.GenerateMarkdown.ToString());
        table.AddRow("ProcessorCount", Environment.ProcessorCount.ToString());

        AnsiConsole.Write(
            new Panel(table)
                .Header("OcrFlow", Justify.Center)
                .Border(BoxBorder.Double)
        );
    }
}