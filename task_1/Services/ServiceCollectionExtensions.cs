using Microsoft.Extensions.DependencyInjection;
using SerializerTests.Implementations;
using SerializerTests.Interfaces;

namespace Services;

public static class ServiceCollectionExtensions
{
    public static void RegisterServicesDependencies(this IServiceCollection services)
    {
        services.AddScoped<IListSerializer, ListSerializer>();
    }
}