using Commerce.Application.Interfaces.Out;
using Commerce.Infrastructure.Persistence;
using Commerce.Domain.Outbox;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace Commerce.Infrastructure.Outbox;

public sealed class EfOutbox(CommerceDbContext db) : IOutbox
{
    private readonly CommerceDbContext _db = db;

    public void Enqueue(string type, string payload)
    {
        OutboxMessage outboxMessage = new() { Type = type, Payload = payload};
        _db.Set<OutboxMessage>().Add(outboxMessage);
    }

    public async Task<IReadOnlyList<OutboxMessage>> GetUnprocessedAsync(int take, CancellationToken ct)
    {
        return await _db.Set<OutboxMessage>()
            .Where(m => m.DispatchedAtUtc == null)
            .OrderBy(m => m.OccurredAtUtc)
            .ThenBy(x => x.Id)
            .Take(take).ToListAsync(ct);
    }
    public void MarkProcessed(IReadOnlyList<OutboxMessage> messages, DateTime proccessedAtUtc)
    {
        foreach (var message in messages)
        {
            message.MarkProcessed(proccessedAtUtc);
        }
    }
}