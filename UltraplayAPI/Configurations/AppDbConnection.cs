using Microsoft.EntityFrameworkCore;
using UP.DataLayer;

namespace UP.Api.Configurations;

public static class AppDbConnection
{
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        return services;
    }
}