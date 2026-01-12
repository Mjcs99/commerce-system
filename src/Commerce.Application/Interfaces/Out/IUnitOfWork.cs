namespace Commerce.Application.Interfaces.Out;
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}