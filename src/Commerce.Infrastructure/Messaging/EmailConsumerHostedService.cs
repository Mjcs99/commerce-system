using Azure.Messaging.ServiceBus;
using Commerce.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
namespace Commerce.Infrastructure.Messaging;

public sealed class EmailConsumerHostedService : ServiceBusConsumerHostedService
{
    public EmailConsumerHostedService(
        IServiceProvider services,
        ILogger<EmailConsumerHostedService> logger,
        ServiceBusClient client,
        IOptionsMonitor<ServiceBusOptions> options)
        : base(services, logger, client, options) { }

    protected override string OptionsName => "Email";
}


