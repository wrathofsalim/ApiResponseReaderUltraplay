using Microsoft.Extensions.DependencyInjection;
using UP.Core.Contracts;
using UP.Core.Contracts.Repositories;
using UP.Repository.Repositories;

namespace UP.Repository;

public static class RepositoryInjection
{
    public static IServiceCollection AddRepositoryInjection(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IBetRepository, BetRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IMatchRepository, MatchRepository>();
        services.AddScoped<IOddRepository, OddRepository>();
        services.AddScoped<ISportRepository, SportRepository>();
        services.AddScoped<IMainRepository, MainRepository>();

        return services;
    }

}