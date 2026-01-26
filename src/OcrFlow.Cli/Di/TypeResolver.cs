using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

public sealed class TypeResolver : ITypeResolver, IDisposable
{
    private readonly ServiceProvider _provider;

    public TypeResolver(ServiceProvider provider)
        => _provider = provider;

    public object? Resolve(Type? type)
        => type is null ? null : _provider.GetRequiredService(type);

    public void Dispose()
        => _provider.Dispose();
}