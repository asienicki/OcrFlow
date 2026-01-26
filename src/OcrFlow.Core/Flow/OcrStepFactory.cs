using Microsoft.Extensions.DependencyInjection;
using OcrFlow.Core.Flow.Abstractions;

namespace OcrFlow.Core.Flow;

public sealed class OcrStepFactory : IOcrStepFactory
{
    private readonly IServiceProvider _services;

    public OcrStepFactory(IServiceProvider services)
    {
        _services = services;
    }

    public T Create<T>() where T : IOcrStep
    {
        return _services.GetRequiredService<T>();
    }
}