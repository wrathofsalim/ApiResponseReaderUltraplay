using Microsoft.Extensions.DependencyInjection;
using UP.Core.Contracts.Services;
using UP.Services.Services;

namespace UP.Services;

public static class ServicesInjection
{
    public static IServiceCollection AddServicesInjection(this IServiceCollection services)
    {
        services.AddScoped<IBetService, BetService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IMatchService, MatchService>();
        services.AddScoped<IOddService, OddService>();
        services.AddScoped<ISportService, SportService>();
        services.AddScoped<IMainService, MainService>();
        services.AddScoped<IRepeatingService, RepeatingService>();

        return services;
    }
}