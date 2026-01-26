using Spectre.Console;

namespace OcrFlow.Cli.Diagnostics;

public static class SpectreExceptionFormatter
{
    public static string Format(Exception ex)
    {
        var writer = new StringWriter();

        var console = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Out = new AnsiConsoleOutput(writer),
            Ansi = AnsiSupport.No
        });

        console.WriteException(
            ex,
            ExceptionFormats.ShowLinks
            | ExceptionFormats.ShortenEverything
        );

        return writer.ToString();
    }
}