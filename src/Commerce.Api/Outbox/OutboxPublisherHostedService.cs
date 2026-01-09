using Commerce.Application.Interfaces.In.Outbox;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Hosting;

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
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _services.CreateScope();
            
            var publisher = scope.ServiceProvider.GetRequiredService<IOutboxPublisher>();

            var count = await publisher.PublishPendingAsync(stoppingToken);

            // if work was done, run again right away
            var delay = count > 0 ? TimeSpan.FromMilliseconds(50)
                                : TimeSpan.FromSeconds(1);

            await Task.Delay(delay, stoppingToken);
        }
      
    }
}