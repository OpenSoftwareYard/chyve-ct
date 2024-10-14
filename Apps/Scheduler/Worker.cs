using System.Threading;
using System.Threading.Tasks;
using EventBus;
using Events;
using Scheduler.EventHandlers;

namespace Scheduler;

public class Worker(ILogger<Worker> logger, IEventBus eventBus) : BackgroundService
{
    private readonly ILogger<Worker> _logger = logger;
    private readonly IEventBus _eventBus = eventBus;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        _eventBus.Subscribe<PlaceZoneIntegrationEvent, PlaceZoneIntegrationEventHandler>();

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
}
