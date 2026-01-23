using OcrFlow.App.Diagnostics;
using PdfSharp.Fonts;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Collections.Concurrent;


AppDomain.CurrentDomain.UnhandledException += (_, e) =>
{
    CrashReporter.Handle(e.ExceptionObject as Exception);
};

TaskScheduler.UnobservedTaskException += (_, e) =>
{
    CrashReporter.Handle(e.Exception);
    e.SetObserved();
};

try
{
    GlobalFontSettings.FontResolver ??= new PdfFontResolver();
    var statuses = new ConcurrentDictionary<int, PageStatus>();
    AnsiConsole.Clear();
    
    PrintHeader();

    var app = new CommandApp<OcrCommand>();
    app.Configure(cfg =>
    {
        cfg.SetApplicationName("ocrpdf");
        cfg.ValidateExamples();
    });

    return app.Run(args);
}
catch (Exception ex)
{
    CrashReporter.Handle(ex);
    return -1;
}

void PrintHeader()
{
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