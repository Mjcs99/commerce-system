using Commerce.Application.Interfaces;
using Commerce.Domain.Entities;
using Commerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Infrastructure.Repositories;

public class EfProductRepository : IProductRepository
{
    private readonly CommerceDbContext _db;

    public EfProductRepository(CommerceDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<Product>> GetAllProductsAsync()
        => await _db.Products
            .AsNoTracking()
            .ToListAsync();

    public async Task<Product?> GetProductByIdAsync(Guid id)
        => await _db.Products
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Id == id);

    public async Task<Product?> GetProductBySkuAsync(string sku)
        => await _db.Products
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Sku == sku);
}
