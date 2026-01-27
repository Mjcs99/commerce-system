using Commerce.Api.Interfaces.Out;
using Commerce.Infrastructure.Persistence;

namespace Commerce.Infrastructure.Repositories;

public class EfCategoryRepository : ICategoryRepository
{
    private readonly CommerceDbContext _db;
    public EfCategoryRepository(CommerceDbContext db)
    {
        _db = db;
    }
    public async Task<IEnumerable<string>> GetCategorySlugs()
    {
        return _db.Category.Select(c => c.Slug);
    }
}