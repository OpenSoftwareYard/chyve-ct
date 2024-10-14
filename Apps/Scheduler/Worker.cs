using System.Threading;
using System.Threading.Tasks;
using EventBus;

namespace Scheduler;

public class Worker(ILogger<Worker> logger, IEventBus eventBus) : BackgroundService
{
    private readonly ILogger<Worker> _logger = logger;
    private readonly IEventBus _eventBus = eventBus;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
