using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace UP.DataLayer;

public static class CreateInitialMigration
{
    public static async Task InitialMigrate(this IApplicationBuilder applicationBuilder)
    {
        try
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();

            var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (context.Database.IsSqlServer())
            {
                context.Database.Migrate();
            }

            await context.SaveChangesAsync();

        }
        catch
        {
            throw new Exception("Migration Failed" + nameof(InitialMigrate));
        }
    }
}