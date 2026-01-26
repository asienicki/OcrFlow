using Spectre.Console;

namespace OcrFlow.Cli.Ui;

public static class SpectraConsoleHelper
{
    public static void PrintHeader()
    {
        AnsiConsole.Clear();

        var figlet = new FigletText("OCR Flow")
        {
            Color = Color.Green,
            Justification = Justify.Center
        };

        var link = new Markup(
            "[grey]https://github.com/asienicki/OcrFlow[/]\n" +
            "[dim]Convert scans into searchable PDFs\r\nExport structured Markdown[/]"
        )
        {
            Justification = Justify.Center
        };

        var content = new Rows(
            figlet,
            new Rule { Style = Style.Parse("grey") },
            link
        );

        var panel = new Panel(content)
        {
            Border = BoxBorder.Double,
            BorderStyle = new Style(Color.Red),
            Padding = new Padding(1, 1, 1, 1)
        };

        AnsiConsole.Write(panel);
    }
}
