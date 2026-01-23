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

    var figlet = new FigletText("OCR Flow")
    {
        Color = Color.Green,
        Justification = Justify.Center
    };

    var panel = new Panel(figlet)
    {
        Border = BoxBorder.Double,
        BorderStyle = new Style(Color.Red),
        Padding = new Padding(1, 1, 1, 1)
    };

    AnsiConsole.Write(panel);

    var app = new CommandApp<OcrCommand>();
    app.Configure(cfg =>
    {
        cfg.SetApplicationName("ocrpdf");
        cfg.ValidateExamples();
    });

    throw new Exception("I got this error during my tests.");

    return app.Run(args);
}
catch (Exception ex)
{
    CrashReporter.Handle(ex);
    return -1;
}