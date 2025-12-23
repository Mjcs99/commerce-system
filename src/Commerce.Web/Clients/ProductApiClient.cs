using Commerce.Contracts.Products;

namespace Commerce.Web.Clients;

public class ProductApiClient : IProductApiClient
{
    private readonly HttpClient _http;

    public ProductApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<IReadOnlyList<ProductDto>> GetAllAsync()
    {
        var products =
            await _http.GetFromJsonAsync<IReadOnlyList<ProductDto>>(
                "/api/v1/products");

        return products ?? [];
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        return await _http.GetFromJsonAsync<ProductDto>(
            $"/api/v1/products/{id}");
    }
}
