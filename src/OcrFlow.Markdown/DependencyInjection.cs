using Microsoft.Extensions.DependencyInjection;
using OcrFlow.Core.Output.Abstractions;
using OcrFlow.Markdown.Output;

namespace OcrFlow.Markdown
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddOcrMarkdown(
            this IServiceCollection services)
        {
            services.AddTransient<IOcrOutputStrategy, MarkdownOutputStrategy>();

            return services;
        }
    }
}
