using System.Runtime.CompilerServices;

namespace Commerce.Application.Interfaces.In.Outbox;
public interface IOutboxPublisher
{
    public Task<int> PublishPendingAsync(CancellationToken stoppingToken);
}