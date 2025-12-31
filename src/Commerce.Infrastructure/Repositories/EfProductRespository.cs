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

    public async Task<Guid> CreateAsync(Product product)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return product.Id;
    }

    public async Task<(IReadOnlyList<Product>, int)> GetPagedAsync(string? searchTerm, int page, int pageSize)
    {
        var query = _db.Products.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(p => p.Name.Contains(searchTerm.ToLower()) || p.Sku.Contains(searchTerm.ToLower()));
        }

        query = query.OrderBy(p => p.Name).ThenBy(p => p.Id);

        var totalCount = await query.CountAsync();
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 200);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return (items, totalCount);
    }



    public async Task<Product?> GetProductByIdAsync(Guid id)
        => await _db.Products
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Id == id);

    public async Task<Product?> GetProductBySkuAsync(string sku)
        => await _db.Products
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Sku == sku);
}
