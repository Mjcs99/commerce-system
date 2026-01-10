using Azure.Messaging.ServiceBus;
using Commerce.Infrastructure.Options;
using Microsoft.Extensions.Options;
namespace Commerce.Api.Messaging;

public sealed class OrdersConsumerHostedService : ServiceBusConsumerHostedService
{
    public OrdersConsumerHostedService(
        IServiceProvider services,
        ILogger<OrdersConsumerHostedService> logger,
        ServiceBusClient client,
        IOptionsMonitor<ServiceBusOptions> options)
        : base(services, logger, client, options) { }

    protected override string OptionsName => "OrderPlaced";
}
