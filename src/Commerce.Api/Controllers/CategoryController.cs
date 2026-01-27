using Microsoft.AspNetCore.Mvc;
using Commerce.Application.Categories.Results;
using Commerce.Application.Interfaces.In;
using Commerce.Domain.Entities;

namespace Commerce.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/categories")]
public class CategoryController : ControllerBase
{   
    private readonly ICategoryService _categoryService;
    
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryResult>>> GetCategories()
    {
        var categories = await _categoryService.GetCategories();
        return Ok(categories);
    }
}