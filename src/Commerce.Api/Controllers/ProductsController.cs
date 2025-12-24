using Commerce.Contracts.Products;
using Commerce.Application.Interfaces;
using Commerce.Application.Products.Queries;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProductsAsync(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var results = await _productService.GetProductsAsync(
            new GetProductsQuery(page, pageSize)
        );

        var dtos = results.Select(r => new ProductDto(
            r.Id,
            r.Name,
            r.Sku,
            r.Price
        ));

        return Ok(dtos);
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
