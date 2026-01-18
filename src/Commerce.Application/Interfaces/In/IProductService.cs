using Commerce.Application.Products.Results;
using Commerce.Application.Products.Commands;
using Commerce.Application.Common;
using Commerce.Application.Products.Queries;
using Commerce.Contracts.Products;

namespace Commerce.Application.Interfaces.In;

public interface IProductService
{
    Task<PagedQueryResult<ProductResult>> GetProductsAsync(GetProductsQuery query, CancellationToken ct);
    Task<AddImageResult> AddImageAsync(AddProductImageCommand command, CancellationToken ct);
    Task<Guid> AddProductAsync(CreateProductCommand command, CancellationToken ct);
    Task<ProductResult> GetProductByIdAsync(Guid productId, CancellationToken ct);
    Task<ProductDetailsDto> GetProductDetailsByIdAsync(Guid productId, CancellationToken ct);
    Task<ProductResult> GetProductBySkuAsync(string sku, CancellationToken ct);
}
