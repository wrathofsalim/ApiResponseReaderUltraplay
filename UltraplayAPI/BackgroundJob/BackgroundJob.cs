using UP.Core.Contracts.Services;

namespace UP.Api.BackgroundJob;

public class BackgroundJob : BackgroundService
{
    private readonly ILogger<BackgroundJob> logger;
    private readonly IServiceScopeFactory serviceScopeFactory;
    private Timer timer;

    public BackgroundJob(ILogger<BackgroundJob> logger, IServiceScopeFactory serviceScopeFactory)
    {
        this.logger = logger;
        this.serviceScopeFactory = serviceScopeFactory;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        timer = new Timer(RepeatCallLink, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

        await Task.CompletedTask;
    }

    private void RepeatCallLink(object state)
    {
        logger.LogInformation("Background task is running.");

        using (var scope = serviceScopeFactory.CreateScope())
        {
            var scopedService = scope.ServiceProvider.GetRequiredService<IRepeatingService>();
            scopedService.CallLink().GetAwaiter().GetResult();
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        timer?.Change(Timeout.Infinite, 0);
        await base.StopAsync(stoppingToken);
    }
}