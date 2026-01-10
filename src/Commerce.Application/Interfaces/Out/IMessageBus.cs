namespace Commerce.Application.Interfaces.Out;
public interface IMessageBus
{
    public Task PublishAsync(Guid messageId, string type, string payload, CancellationToken ct);
}