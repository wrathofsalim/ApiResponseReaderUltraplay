using UP.Repository;
using UP.Services;

namespace UP.Api.Configurations;

public static class RegisterServices
{
    public static IServiceCollection Register(this IServiceCollection services)
    {
        RepositoryInjection.AddRepositoryInjection(services);

        ServicesInjection.AddServicesInjection(services);

        services.AddHttpContextAccessor();

        return services;
    }
}