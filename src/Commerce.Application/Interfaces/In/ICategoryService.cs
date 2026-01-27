using Commerce.Application.Categories.Results;
namespace Commerce.Application.Interfaces.In;
public interface ICategoryService
{
    public Task<CategoryResult> GetCategories();    
}
