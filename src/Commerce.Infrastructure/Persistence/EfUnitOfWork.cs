using Commerce.Application.Interfaces.Out;

namespace Commerce.Infrastructure.Persistence;

public sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly CommerceDbContext _db;
    public EfUnitOfWork(CommerceDbContext db)
    {
        _db = db;
    }
    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);
}