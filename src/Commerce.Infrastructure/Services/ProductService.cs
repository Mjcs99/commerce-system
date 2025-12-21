using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commerce.Application.DTOs;
using Commerce.Application.Interfaces;

namespace Commerce.Infrastructure.Services;

public class ProductService : IProductService
{
    private static readonly List<ProductDto> Products =
    [
        new ProductDto(Guid.Parse("11111111-1111-1111-1111-111111111111"), "SKU-005", "Starter Widget", 19.99m, "CAD"),
        new ProductDto(Guid.Parse("22222222-2222-2222-2222-222222222222"), "SKU-002", "Pro Widget", 49.99m, "CAD"),
    ];

    public Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        => Task.FromResult<IEnumerable<ProductDto>>(Products);

    public Task<ProductDto?> GetProductByIdAsync(Guid productId)
        => Task.FromResult(Products.SingleOrDefault(p => p.ProductId == productId));
}
