using Microsoft.Extensions.DependencyInjection;
using OcrFlow.Core.Output.Abstractions;
using OcrFlow.Pdf.Output;

namespace OcrFlow.Pdf
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddOcrPdf(
            this IServiceCollection services)
        {
            services.AddTransient<IOcrOutputStrategy, PdfOutputStrategy>();

            return services;
        }
    }
}
