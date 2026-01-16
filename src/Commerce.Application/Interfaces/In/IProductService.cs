using Commerce.Application.Products.Results;
using Commerce.Application.Products.Commands;
using Commerce.Application.Common;
using Commerce.Application.Products.Queries;

namespace Commerce.Application.Interfaces.In;

public interface IProductService
{
    Task<PagedQueryResult<ProductResult>> GetProductsAsync(GetProductsQuery query, CancellationToken ct);
    Task<AddImageResult> AddImageAsync(AddProductImageCommand command, CancellationToken ct);
<<<<<<< HEAD
    Task<Guid> AddProductAsync(CreateProductCommand command, CancellationToken ct);
=======
    Task<AddProductResult> AddProductAsync(CreateProductCommand command, CancellationToken ct);
>>>>>>> 2c1c6145c851b9f6ff3f8050d464ae51db9a7269
    Task<ProductResult> GetProductByIdAsync(Guid productId, CancellationToken ct);
    Task<ProductResult> GetProductBySkuAsync(string sku, CancellationToken ct);
}
