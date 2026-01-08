using Commerce.Application.Interfaces.Out;
using Commerce.Infrastructure.Persistence;
using Commerce.Infrastructure.Persistence.Outbox;
namespace Commerce.Infrastructure.Repositories;

public class EfOutbox(CommerceDbContext db) : IOutbox
{
    private readonly CommerceDbContext _db = db;

    public void Enqueue(string type, string payload)
    {
        OutboxMessage outboxMessage = new() { Type = type, Payload = payload};
        _db.OutboxMessages.Add(outboxMessage);
    }
}