using Commerce.Contracts.Products;

namespace Commerce.Application.Interfaces;

public interface IProductService
{
    Task<IReadOnlyList<ProductDto>> GetAllProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(Guid productId);
    Task<ProductDto?> GetProductBySkuAsync(string sku);
    
}
