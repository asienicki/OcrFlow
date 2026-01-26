using Microsoft.Extensions.DependencyInjection;
using OcrFlow.Core.Flow;
using OcrFlow.Core.Flow.Abstractions;
using OcrFlow.Core.Flow.Steps;
using OcrFlow.Core.Flow.Steps.Execution;

namespace OcrFlow.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddOcrCore(
        this IServiceCollection services)
    {
        _ = services.AddSingleton<IOcrStepFactory, OcrStepFactory>();

        _ = services.AddTransient<LoadImageStep>();
        _ = services.AddTransient<RotateStep>();
        _ = services.AddTransient<NormalizeDpiStep>();
        _ = services.AddTransient<DeskewStep>();
        _ = services.AddTransient<TesseractOcrStep>();
        _ = services.AddTransient<OcrProcess>();

        return services;
    }
}