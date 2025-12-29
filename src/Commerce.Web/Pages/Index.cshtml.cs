using Commerce.Contracts.Common;   // for PagedResult<T>
using Commerce.Contracts.Products;
using Commerce.Web.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Commerce.Web.Pages.Products;

public class IndexModel : PageModel
{
    private readonly IProductApiClient _api;

    public IReadOnlyList<ProductDto> Products { get; private set; } = [];

    [BindProperty(SupportsGet = true, Name = "p")]
    public int CurrentPage { get; set; } = 1;

   [BindProperty(SupportsGet = true, Name = "q")]
    public string? SearchTerm { get; set; }
    public int PageSize { get; private set; } = 20;
    public int TotalCount { get; private set; }
    public int TotalPages { get; private set; }

    public bool HasPrev => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    public IndexModel(IProductApiClient api) => _api = api;

    public async Task OnGetAsync()
    {
        CurrentPage = Math.Max(CurrentPage, 1);
        
        SearchTerm = string.IsNullOrWhiteSpace(SearchTerm) ? null : SearchTerm.Trim();
        var result = await _api.GetPageAsync(CurrentPage, SearchTerm);

        Products = result.Items;
        TotalCount = result.TotalCount;
        TotalPages = result.TotalPages;
    }
}


