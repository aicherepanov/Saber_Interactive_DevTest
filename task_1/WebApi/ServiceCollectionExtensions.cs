using Application.Features.ListSerializer.Handlers;
using MediatR;
using Services;

namespace ListSerializer.Web;

public static class ServiceCollectionExtensions
{
    public static void RegisterProjectDependencies(this IServiceCollection services)
    {
        services.AddMediatR(typeof(GetDeserializedListQueryHandler), typeof(GetSerializedListQueryHandler));
        services.RegisterServicesDependencies();
    }
}