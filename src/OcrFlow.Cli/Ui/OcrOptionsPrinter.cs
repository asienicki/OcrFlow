using OcrFlow.Core.Flow.Models.Runtime;
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
            .AddColumn("Value")
            .AddRow("InputDir", o.InputDir)
            .AddRow("Languages", string.Join(", ", o.Languages))
            .AddRow("Extensions", string.Join(", ", o.ImageExtensions))
            .AddRow("GeneratePdf", o.GeneratePdf.ToString())
            .AddRow("OnlyMarkdown", o.GenerateMarkdown.ToString())
            .AddRow("ProcessorCount", Environment.ProcessorCount.ToString());

        AnsiConsole.Write(
            new Panel(table)
                .Header("OcrFlow", Justify.Center)
                .Border(BoxBorder.Double)
        );
    }
}