using Commerce.Application.Interfaces;
using Commerce.Domain.Entities;
using Commerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Commerce.Application.Products.Results;
using Commerce.Infrastructure.Images;

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

    public async Task<(IReadOnlyList<Product>, int)> GetPagedAsync(
    string? searchTerm,
    int page,
    int pageSize)
    {
        IQueryable<Product> query = _db.Products
            .AsNoTracking()
            .Include(p => p.Images);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            query = query.Where(p =>
                p.Name.ToLower().Contains(term) ||
                p.Sku.ToLower().Contains(term));
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



    public async Task AddImageAsync(Guid productId, string blobName, Guid imageId, bool makePrimary)
    {
        var product = await _db.Products
            .Include(p => p.Images)
            .SingleOrDefaultAsync(p => p.Id == productId);

        if (product is null)
            throw new InvalidOperationException("Product not found.");

        var newImage = product.AddImage(blobName, imageId, makePrimary);
        _db.Entry(newImage).State = EntityState.Added;

        await _db.SaveChangesAsync();

    }

    public async Task<Product?> GetProductByIdAsync(Guid id)
        => await _db.Products
            .SingleOrDefaultAsync(p => p.Id == id);

    public async Task<Product?> GetProductBySkuAsync(string sku)
        => await _db.Products
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Sku == sku);
}
