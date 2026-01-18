using Commerce.Domain.Entities;
using Commerce.Application.Products.Results;
namespace Commerce.Application.Interfaces.Out;

public interface IProductRepository
{
    Task<(IReadOnlyList<Product> items, int totalCount)> GetPagedAsync(string? searchTerm, string? categorySlug, int page, int pageSize, CancellationToken ct);
    Task<Guid?> GetCategoryIdBySlugAsync(string categorySlug, CancellationToken ct);
    Task<Product?> GetProductByIdAsync(Guid id, CancellationToken ct);
    Task<Product?> GetProductDetailsByIdAsync(Guid id, CancellationToken ct);
    Task<Product?> GetProductBySkuAsync(string sku, CancellationToken ct);
    Task<Guid> CreateAsync(Product product, CancellationToken ct);
    Task AddImageAsync(Guid productId, string blobName, Guid imageId, bool makePrimary, CancellationToken ct);
}
