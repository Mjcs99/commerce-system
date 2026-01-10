namespace Commerce.Application.Interfaces.Out;
using Commerce.Domain.Outbox;
public interface IOutbox
{
    void Enqueue(string type, string payload);
    Task<IReadOnlyList<OutboxMessage>> GetUnprocessedAsync(int take, CancellationToken ct);
    public void MarkProcessed(IReadOnlyList<OutboxMessage> messages, DateTime proccessedAtUtc);
}