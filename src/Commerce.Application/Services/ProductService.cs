using Commerce.Application.Products.Results;
using Commerce.Application.Interfaces;
using Commerce.Domain.Entities;
using Commerce.Application.Products.Queries;

namespace Commerce.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;

    public ProductService(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<IReadOnlyList<ProductResult>> GetProductsAsync(GetProductsQuery query)
    {
        var products = await _repo.GetPagedAsync(query.Page, query.PageSize);
        return products.Select(Map).ToList();
    }

    public async Task<ProductResult?> GetProductByIdAsync(Guid productId)
    {
        var product = await _repo.GetProductByIdAsync(productId);
        return product is null ? null : Map(product);
    }

    private static ProductResult Map(Product p)
        => new(p.Id, p.Name, p.Sku, p.PriceAmount);

    public async Task<ProductResult?> GetProductBySkuAsync(string sku)
    {
        if (string.IsNullOrWhiteSpace(sku)) return null; 
        var product = await _repo.GetProductBySkuAsync(sku);
        return product is null ? null : Map(product);
    }
}
