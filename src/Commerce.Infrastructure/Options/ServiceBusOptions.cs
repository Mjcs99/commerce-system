using Microsoft.Identity.Client;

namespace Commerce.Infrastructure.Options;
public sealed class ServiceBusOptions
{
    public string ConnectionString { get; init; } = default!;
    public string OrdersTopic { get; init; } = default!;
    public string Subscription { get; init; } = default!;
}