using Commerce.Application.Interfaces.In.Outbox;

namespace Commerce.Api.Outbox;

public class OutboxPublisherHostedService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<OutboxPublisherHostedService> _logger;
    public OutboxPublisherHostedService(IServiceProvider services, ILogger<OutboxPublisherHostedService> logger)
    {
        _services = services;
        _logger = logger;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbox publisher hosted service started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _services.CreateScope();
            
            var publisher = scope.ServiceProvider.GetRequiredService<IOutboxPublisher>();

            var published = await publisher.PublishPendingAsync(stoppingToken);
            if (published > 0)
            {
                _logger.LogInformation("Outbox publisher published {published} message(s).", published);
            }
            // Fix for when swapping to live db
            // if work was done, run again right away
            var delay = published > 0 ? TimeSpan.FromMilliseconds(50)
                                : TimeSpan.FromSeconds(1);

            await Task.Delay(delay, stoppingToken);
        }
      
    }
}