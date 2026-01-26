using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OcrFlow.Application;
using OcrFlow.Cli.Bootstrap;
using OcrFlow.Cli.Commands;
using OcrFlow.Cli.Diagnostics;
using OcrFlow.Core;
using OcrFlow.Core.Flow.Models.Options;
using OcrFlow.Core.Output.Abstractions;
using OcrFlow.Markdown;
using OcrFlow.Markdown.Output;
using OcrFlow.Pdf;
using OcrFlow.Pdf.Output;
using PdfSharp.Fonts;
using Spectre.Console.Cli;

AppDomain.CurrentDomain.UnhandledException += (_, e) =>
{
    if (e.ExceptionObject is Exception ex)
    {
        CrashReporter.Handle(ex);
    }
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

    var host = Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(config =>
        {
            config
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}.json",
                    optional: true)
                .AddEnvironmentVariables();
        })
        .ConfigureServices((ctx, _) =>
        {
            services.Configure<OcrFlowOptions>(
                ctx.Configuration.GetSection("OcrFlow"));

            services
                    .AddOcrCore()
                    .AddOcrMarkdown()
                    .AddOcrApplication()
                    .AddOcrPdf();

            services.AddTransient<OcrCommand>();

            services.Configure<OcrFlowOptions>(ctx.Configuration.GetSection("OcrFlow"));

            services.AddSingleton<OcrRunContext>();
            services.AddSingleton<OcrRunOptionsBuilder>();

            services.AddSingleton<IOutputFinalizer, PdfMergeFinalizer>();
            services.AddSingleton<IOutputFinalizer, MarkdownMergeFinalizer>();
        })
        .Build();

    var registrar = new TypeRegistrar(services);
    var app = new CommandApp<OcrCommand>(registrar);

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
