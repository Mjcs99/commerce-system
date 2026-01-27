using Commerce.Application.Categories;
using Commerce.Application.Products.Queries;

namespace Commerce.Api.Interfaces.Out;

public interface ICategoryRepository
{
    public Task<IEnumerable<string>> GetCategorySlugs();
}