using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace OcrFlow.Cli.Di;

public sealed class TypeResolver : ITypeResolver, IDisposable
{
    private readonly ServiceProvider _provider;

    public TypeResolver(ServiceProvider provider)
    {
        _provider = provider;
    }

    public void Dispose()
    {
        _provider.Dispose();
    }

    public object? Resolve(Type? type)
    {
        return type is null ? null : _provider.GetRequiredService(type);
    }
}