using Commerce.Application.DTOs;
using Commerce.Application.Interfaces;
using Commerce.Domain.Entities;

namespace Commerce.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;

    public ProductService(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<IReadOnlyList<ProductDto>> GetAllProductsAsync()
    {
        var products = await _repo.GetAllProductsAsync();
        return products.Select(Map).ToList();
    }

    public async Task<ProductDto?> GetProductByIdAsync(Guid productId)
    {
        var product = await _repo.GetProductByIdAsync(productId);
        return product is null ? null : Map(product);
    }

    private static ProductDto Map(Product p)
        => new(p.Id, p.Sku, p.Name, p.PriceAmount, p.Currency);

    public async Task<ProductDto?> GetProductBySkuAsync(string sku)
    {
        if (string.IsNullOrWhiteSpace(sku)) return null; 
        var product = await _repo.GetProductBySkuAsync(sku);
        return product is null ? null : Map(product);
    }
}
