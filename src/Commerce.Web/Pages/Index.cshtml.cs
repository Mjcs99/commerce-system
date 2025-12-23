using Commerce.Contracts.Products;
using Commerce.Web.Clients;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Commerce.Web.Pages.Products;

public class IndexModel : PageModel
{
    private readonly IProductApiClient _api;

    public IReadOnlyList<ProductDto> Products { get; private set; } = [];

    public IndexModel(IProductApiClient api)
    {
        _api = api;
    }

    public async Task OnGetAsync()
    {
        Products = await _api.GetAllAsync();
    }
}
