using Commerce.Domain.Entities;

namespace Commerce.Application.Interfaces;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(Guid id);
    Task<Product?> GetProductBySkuAsync(string sku);
}
