namespace Commerce.Application.Interfaces.In;

public interface IIntegrationEventHandler
{
    public bool CanHandle(string type);
    public Task HandleAsync(string type, string payload, CancellationToken ct);
}
