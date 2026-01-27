using Commerce.Api.Interfaces.Out;
using Commerce.Application.Categories;
using Commerce.Application.Categories.Results;
using Commerce.Application.Interfaces.In;

namespace Commerce.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryResult> GetCategories()
    {
        var slugs = await _categoryRepository.GetCategorySlugs();
        return new CategoryResult(slugs.ToList());
    }

}