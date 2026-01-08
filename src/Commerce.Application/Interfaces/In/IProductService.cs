using Commerce.Application.Products.Results;
using Commerce.Application.Products.Commands;
using Commerce.Application.Common;
using Commerce.Application.Products.Queries;

namespace Commerce.Application.Interfaces.In;

public interface IProductService
{
    Task<PagedQueryResult<ProductResult>> GetProductsAsync(GetProductsQuery query);
    Task<AddImageResult> AddImageAsync(AddProductImageCommand command);
    Task<AddProductResult> AddProductAsync(CreateProductCommand command);
    Task<ProductResult?> GetProductByIdAsync(Guid productId);
    Task<ProductResult?> GetProductBySkuAsync(string sku);
}
