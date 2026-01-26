using Microsoft.Extensions.DependencyInjection;
using OcrFlow.Core.Flow;
using OcrFlow.Core.Flow.Abstractions;
using OcrFlow.Core.Flow.Steps;
using OcrFlow.Core.Flow.Steps.Execution;

namespace OcrFlow.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddOcrCore(
        this IServiceCollection services)
        {
            services.AddSingleton<IOcrStepFactory, OcrStepFactory>();

            services.AddTransient<LoadImageStep>();
            services.AddTransient<RotateStep>();
            services.AddTransient<NormalizeDpiStep>();
            services.AddTransient<DeskewStep>();
            services.AddTransient<TesseractOcrStep>();
            services.AddTransient<OcrProcess>();

            return services;
        }
    }
}
