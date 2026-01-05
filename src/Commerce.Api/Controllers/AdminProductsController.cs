using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Commerce.Application.Interfaces;
using Commerce.Application.Products.Commands;
using Commerce.Contracts.Products;
using Commerce.Application.Products.Results;
using Commerce.Api.Requests;

namespace Commerce.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/admin/products")]
//[Authorize(Policy = "AdminOnly")] 
public class AdminProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public AdminProductsController(IProductService productService)
    {
        _productService = productService;
    }
    // What do i want this to return? Come back to this later
    [HttpPost]
    public async Task<ActionResult<ProductDto>> AddProductAsync([FromBody] CreateProductDto dto, CancellationToken ct)
    {
        var result = await _productService.AddProductAsync(
            new CreateProductCommand(dto.Name, dto.Sku, dto.CategorySlug, dto.Price));

        if (!result.Success)
            return BadRequest(result.ErrorMessage);

        var product = await _productService.GetProductByIdAsync(result.ProductId);
        if (product is null)
            return Problem("Product was created but could not be loaded.");

        var productDto = new ProductDto(product.Id, product.Sku, product.Name, product.Price);

        return CreatedAtRoute(
            "GetProductById",
            new { version = "1.0", id = product.Id },
            productDto);
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
                request.IsPrimary));

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

}
