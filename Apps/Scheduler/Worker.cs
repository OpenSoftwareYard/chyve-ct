using System.Threading;
using System.Threading.Tasks;
using Scheduler.EventHandlers;

namespace Scheduler;

public class Worker(ILogger<Worker> logger, PlaceZoneHandler placeZoneHandler) : BackgroundService
{
    private readonly ILogger<Worker> _logger = logger;
    private readonly TimeSpan _period = TimeSpan.FromSeconds(5);
    private readonly PlaceZoneHandler _placeZoneHandler = placeZoneHandler;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

        using PeriodicTimer timer = new(_period);
        while (
            !stoppingToken.IsCancellationRequested &&
            await timer.WaitForNextTickAsync(stoppingToken)
        )
        {
            try
            {
                await _placeZoneHandler.PlaceUnscheduledZones();
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Failed to place unscheduled zones {exception}", ex);
            }
        }
    }
}
