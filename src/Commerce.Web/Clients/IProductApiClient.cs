using Commerce.Contracts.Common;
using Commerce.Contracts.Products;

namespace Commerce.Web.Clients;

public interface IProductApiClient
{
    Task<PagedResult<ProductDto>> GetPageAsync(int page = 1, string? searchTerm = null);
    Task<ProductDto?> GetByIdAsync(Guid id);
}
