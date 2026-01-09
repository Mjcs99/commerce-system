namespace Commerce.Application.Interfaces.Out;
public interface IMessageBus
{
    public Task PublishAsync(string type, string payload, CancellationToken ct);
}