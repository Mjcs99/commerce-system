using Commerce.Domain.Entities;
using Commerce.Application.Products.Results;
namespace Commerce.Application.Interfaces;

public interface IProductRepository
{
    Task<(IReadOnlyList<Product>, int TotalCount)> GetPagedAsync(string? searchTerm, int page, int pageSize);
    Task<Product?> GetProductByIdAsync(Guid id);
    Task<Product?> GetProductBySkuAsync(string sku);
    Task<Guid> CreateAsync(Product product);
    Task AddImageAsync(Guid productId, string blobName, Guid imageId, bool makePrimary);
}
