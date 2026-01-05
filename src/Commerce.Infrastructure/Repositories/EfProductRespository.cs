using Commerce.Application.Interfaces;
using Commerce.Application.Products.Commands;
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

    public async Task<(IReadOnlyList<Product> items, int totalCount)> GetPagedAsync(
        string? searchTerm,
        string? categorySlug,
        int page,
        int pageSize)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 200);

        Guid? categoryId = null;

        if (!string.IsNullOrWhiteSpace(categorySlug))
        {
            categoryId = await _db.Category
                .AsNoTracking()
                .Where(c => c.Slug == categorySlug) 
                .Select(c => (Guid?)c.Id)
                .SingleOrDefaultAsync();

            if (categoryId is null)
                return (Array.Empty<Product>(), 0);
        }

        IQueryable<Product> baseQuery = _db.Products.AsNoTracking();

        if (categoryId is not null)
            baseQuery = baseQuery.Where(p => p.CategoryId == categoryId.Value);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            baseQuery = baseQuery.Where(p =>
                EF.Functions.Like(p.Name, $"%{searchTerm}%") ||
                EF.Functions.Like(p.Sku, $"%{searchTerm}%"));
        }

        var totalCount = await baseQuery.CountAsync();

        var items = await baseQuery
            .OrderBy(p => p.Name).ThenBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(p => p.Images.Where(i => i.IsPrimary))  
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

    public async Task<Guid?> GetCategoryIdBySlugAsync(string categorySlug)
    {
        var category = await _db.Category
            .AsNoTracking()
            .SingleOrDefaultAsync(c => c.Slug == categorySlug);
        return category?.Id;
    }
}
