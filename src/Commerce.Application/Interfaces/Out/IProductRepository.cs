using Commerce.Domain.Entities;

namespace Commerce.Application.Interfaces;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetPagedAsync(int page, int pageSize);
    Task<Product?> GetProductByIdAsync(Guid id);
    Task<Product?> GetProductBySkuAsync(string sku);
    //Task AddAsync(Product product);
}
