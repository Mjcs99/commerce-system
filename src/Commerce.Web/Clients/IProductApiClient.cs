using Commerce.Contracts.Products;

namespace Commerce.Web.Clients;

public interface IProductApiClient
{
    Task<IReadOnlyList<ProductDto>> GetAllAsync();
    Task<ProductDto?> GetByIdAsync(Guid id);
}
