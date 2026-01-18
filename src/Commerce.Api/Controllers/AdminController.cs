using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Commerce.Application.Interfaces.In;
using Commerce.Application.Products.Commands;
using Commerce.Contracts.Products;
using Commerce.Application.Products.Results;
using Commerce.Api.Requests;

namespace Commerce.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Authorize(Policy = "AdminOnly")] 
[Route("api/v{version:apiVersion}/admin/products")]

public class AdminController : ControllerBase
{
    private readonly IProductService _productService;

    public AdminController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public async Task<ActionResult<ProductSummaryDto>> AddProductAsync([FromBody] CreateProductDto dto, CancellationToken ct)
    {
        var id = await _productService.AddProductAsync(dto, ct);

        var product = await _productService.GetProductByIdAsync(id, ct);
        if (product is null)
            return Problem("Product was created but could not be loaded.");

        var productDto = new ProductSummaryDto(product.Id, product.Sku, product.Name, product.Price);

        return CreatedAtRoute(
            "GetProductById",
            new { version = "1.0", id = product.Id },
            productDto
            );
    }

    [HttpPost("{productId:guid}/images")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<AddImageResult>> UploadImageAsync(
        [FromRoute] Guid productId,
        [FromForm] UploadProductImageRequest request,
        CancellationToken ct = default)
    {
        if (request.File is null || request.File.Length == 0)
            return BadRequest("File is required.");

        await using var stream = request.File.OpenReadStream();

        var result = await _productService.AddImageAsync(
            new AddProductImageCommand(
                productId,
                stream,
                Path.GetFileName(request.File.FileName),
                request.File.ContentType,
                request.IsPrimary), ct);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}
