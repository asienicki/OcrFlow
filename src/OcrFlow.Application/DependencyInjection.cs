using Microsoft.Extensions.DependencyInjection;
using OcrFlow.Application.Output;
using OcrFlow.Application.Services;

namespace OcrFlow.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddOcrApplication(
            this IServiceCollection services)
        {
            services.AddTransient<OcrOutputStrategies>();
            services.AddTransient<IOcrApplicationService, OcrApplicationService>();

            return services;
        }
    }
}
