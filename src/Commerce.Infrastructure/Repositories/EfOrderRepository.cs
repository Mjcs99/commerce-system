using System.Threading.Tasks;
using Commerce.Application.Interfaces.Out;
using Commerce.Domain.Entities;
using Commerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Infrastructure.Repositories;

public class EfOrderRepository : IOrderRepository
{
    private readonly CommerceDbContext _db;

    public EfOrderRepository(CommerceDbContext db)
    {
        _db = db;
    }
    
    public void AddOrder(Order order)
    {
        _db.Orders.Add(order);
    }

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _db.Orders
            .Include(o => o.Items)              
            .SingleOrDefaultAsync(o => o.Id == id, ct);
    }
}