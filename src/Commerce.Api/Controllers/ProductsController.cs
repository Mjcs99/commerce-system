using Commerce.Contracts.Products;
using Commerce.Contracts.Common;
using Commerce.Application.Interfaces.In;
using Commerce.Application.Products.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<ProductSummaryDto>>> GetAllProductsAsync(
        [FromQuery] string? searchTerm,
        [FromQuery] List<string>? category,
        CancellationToken ct,
        [FromQuery] int page = 1)
    {
        const int pageSize = 20;
        var result = await _productService.GetProductsAsync(
            new GetProductsQuery(
                string.IsNullOrWhiteSpace(searchTerm) ? null : searchTerm.Trim(),
                category?.Count == 0 ? null : category,
                Math.Max(page, 1),
                pageSize
            ), ct);

        var dtos = result.Items.Select(p => new ProductSummaryDto(
            p.Id,
            p.Sku,
            p.Name,
            p.Price,
            p.PrimaryImageUrl
        )).ToList();

        return Ok(new PagedResult<ProductSummaryDto>(
            dtos,
            page,
            result.TotalCount,
            pageSize
        ));
    }

    [HttpGet("{id:guid}/details")]
    public async Task<ActionResult<ProductDetailsDto>> GetProductDetailsByIdAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var product = await _productService.GetProductDetailsByIdAsync(id, ct);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpGet("{id:guid}", Name = "GetProductById")]
    public async Task<ActionResult<ProductSummaryDto>> GetProductByIdAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var product = await _productService.GetProductByIdAsync(id, ct);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpGet("by-sku/{sku}")]
    public async Task<ActionResult<ProductSummaryDto>> GetProductBySkuAsync(string sku, CancellationToken ct)
    {
        var product = await _productService.GetProductBySkuAsync(sku, ct);
        return product is null ? NotFound() : Ok(product);
    }

    
}
