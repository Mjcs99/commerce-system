using Commerce.Application.Interfaces.Out;
using Commerce.Domain.Entities;

namespace Commerce.Infrastructure.Repositories;

public class EfOrderRepository : IOrderRepository
{
    public async Task AddOrderAsync(Order order)
    {
        throw new NotImplementedException();
    }
}