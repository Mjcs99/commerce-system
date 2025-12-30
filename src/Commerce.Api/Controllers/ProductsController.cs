using Commerce.Contracts.Products;
using Commerce.Contracts.Common;
using Commerce.Application.Interfaces;
using Commerce.Application.Products.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Commerce.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<ProductDto>>> GetAllProductsAsync(
        [FromQuery] string? searchTerm,
        [FromQuery] int page = 1
        )
    {
        const int pageSize = 20;
        var result = await _productService.GetProductsAsync(
            new GetProductsQuery(
                string.IsNullOrWhiteSpace(searchTerm) ? null : searchTerm.Trim(),
                Math.Max(page, 1),
                pageSize
            ));

        var dtos = result.Items.Select(p => new ProductDto(
            p.Id,
            p.Name,
            p.Sku,
            p.Price
        )).ToList();

        return Ok(new PagedResult<ProductDto>(
            dtos,
            page,
            result.TotalCount,
            pageSize
        ));
    }


    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductDto>> GetProductByIdAsync(Guid id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        return product is null ? NotFound() : Ok(product);
    }
    [HttpGet("by-sku/{sku}")]
    public async Task<ActionResult<ProductDto>> GetProductBySkuAsync(string sku)
    {
        var product = await _productService.GetProductBySkuAsync(sku);
        return product is null ? NotFound() : Ok(product);
    }
}
