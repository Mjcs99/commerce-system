using Commerce.Application.Products.Results;
using Commerce.Application.Products.Commands;
using Commerce.Application.Products.Queries;

namespace Commerce.Application.Interfaces;

public interface IProductService
{
    Task<IReadOnlyList<ProductResult>> GetProductsAsync(GetProductsQuery query);
    Task<ProductResult?> GetProductByIdAsync(Guid productId);
    Task<ProductResult?> GetProductBySkuAsync(string sku);
    //Task<Guid> CreateProductAsync(CreateProductCommand command);
    
}
