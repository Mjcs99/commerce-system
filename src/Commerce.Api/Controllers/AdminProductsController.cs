using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Commerce.Application.Interfaces;
namespace Commerce.Application.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/admin/products")] 
[Authorize(Policy = "AdminOnly")]
public class AdminProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public AdminProductsController(IProductService productService)
    {
         _productService = productService;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> AddProductAsync([FromBody] CreateProductDto createDto)
    {
        var productId = await _productService.AddProductAsync(createDto);
        var productDto = new ProductDto(
            product.Id,
            product.Name,
            product.Sku,
            product.Price
        );

        return CreatedAtAction(nameof(GetProductByIdAsync), new { id = product.Id }, productDto);
    }
 }