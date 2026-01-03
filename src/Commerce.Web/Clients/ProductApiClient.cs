using Commerce.Contracts.Common;
using Commerce.Contracts.Products;

namespace Commerce.Web.Clients;

public class ProductApiClient : IProductApiClient
{
    private readonly HttpClient _http;

    public ProductApiClient(HttpClient http) => _http = http;

 

    public async Task<ProductDto?> GetByIdAsync(Guid id)
        => await _http.GetFromJsonAsync<ProductDto>($"/api/v1/products/{id}");

    public async Task<PagedResult<ProductDto>> GetPageAsync(int page, string? searchTerm = null)
    {
        const int pageSize = 20;

        var q = string.IsNullOrWhiteSpace(searchTerm)
            ? ""
            : $"&searchTerm={Uri.EscapeDataString(searchTerm)}";

        var url = $"/api/v1/products?page={page}&pageSize={pageSize}{q}";

        return await _http.GetFromJsonAsync<PagedResult<ProductDto>>(url)
            ?? new PagedResult<ProductDto>(Array.Empty<ProductDto>(), page, 0, pageSize);
    }

}

