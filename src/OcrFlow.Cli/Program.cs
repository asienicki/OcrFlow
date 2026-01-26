using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OcrFlow.Application;
using OcrFlow.Cli.Bootstrap;
using OcrFlow.Cli.Commands;
using OcrFlow.Cli.Di;
using OcrFlow.Cli.Diagnostics;
using OcrFlow.Cli.Ui;
using OcrFlow.Core;
using OcrFlow.Core.Flow.Models.Options;
using OcrFlow.Core.Output.Abstractions;
using OcrFlow.Infrastructure.Pdf;
using OcrFlow.Markdown;
using OcrFlow.Markdown.Output;
using OcrFlow.Pdf;
using OcrFlow.Pdf.Output;
using PdfSharp.Fonts;
using Spectre.Console.Cli;

internal static class Program
{
    public static async Task<int> Main(string[] args)
    {
        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        {
            if (e.ExceptionObject is Exception ex)
                CrashReporter.Handle(ex);
        };

        TaskScheduler.UnobservedTaskException += (_, e) =>
        {
            CrashReporter.Handle(e.Exception);
            e.SetObserved();
        };

        try
        {
            GlobalFontSettings.FontResolver = new PdfFontResolver();
            SpectraConsoleHelper.PrintHeader();

            IServiceCollection services = new ServiceCollection();

            _ = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(config =>
                {
                    _ = config
                        .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"), optional: false)
                        .AddJsonFile(
                            Path.Combine(
                                AppContext.BaseDirectory,
                                $"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}.json"),
                            optional: true)
                        .AddEnvironmentVariables();
                })
                .ConfigureServices((ctx, _) =>
                {
                    _ = services.Configure<OcrFlowOptions>(
                        ctx.Configuration.GetSection("OcrFlow"));

                    _ = services
                        .AddOcrCore()
                        .AddOcrMarkdown()
                        .AddOcrApplication()
                        .AddOcrPdf();

                    _ = services.AddTransient<OcrCommand>();
                    _ = services.AddSingleton<OcrRunContext>();
                    _ = services.AddSingleton<OcrRunOptionsBuilder>();

                    _ = services.AddSingleton<IOutputFinalizer, PdfMergeFinalizer>();
                    _ = services.AddSingleton<IOutputFinalizer, MarkdownMergeFinalizer>();
                })
                .Build();

            var registrar = new TypeRegistrar(services);
            var app = new CommandApp<OcrCommand>(registrar);

            app.Configure(cfg =>
            {
                _ = cfg.SetApplicationName("ocrpdf");
                _ = cfg.ValidateExamples();
            });

            return await app.RunAsync(args);
        }
        catch (Exception ex)
        {
            CrashReporter.Handle(ex);
            return -1;
        }
    }
}
