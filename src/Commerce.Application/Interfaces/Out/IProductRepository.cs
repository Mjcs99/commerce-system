using Commerce.Domain.Entities;
using Commerce.Application.Products.Results;
namespace Commerce.Application.Interfaces.Out;

public interface IProductRepository
{
    Task<(IReadOnlyList<Product> items, int totalCount)> GetPagedAsync(string? searchTerm, string? categorySlug, int page, int pageSize);
    Task<Guid?> GetCategoryIdBySlugAsync(string categorySlug);
    Task<Product?> GetProductByIdAsync(Guid id);
    Task<Product?> GetProductBySkuAsync(string sku);
    Task<Guid> CreateAsync(Product product);
    Task AddImageAsync(Guid productId, string blobName, Guid imageId, bool makePrimary);
}
