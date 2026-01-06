using Commerce.Web.Clients;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
var builder = WebApplication.CreateBuilder(args);

// Razor Pages + Identity UI 
builder.Services
    .AddRazorPages()
    .AddMicrosoftIdentityUI();

// Auth (CIAM / Entra External ID)
builder.Services
    .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi()   
    .AddInMemoryTokenCaches();

builder.Services.PostConfigure<OpenIdConnectOptions>(
    OpenIdConnectDefaults.AuthenticationScheme,
    options =>
    {
        options.ResponseType = "code";
        options.UsePkce = true;
    });
builder.Services.AddAuthorization();

// API client
var apiBaseUrl = builder.Configuration["ApiSettings:CommerceApiBaseUrl"]
    ?? throw new InvalidOperationException("ApiSettings:CommerceApiBaseUrl is missing in configuration.");

builder.Services.AddHttpClient<IProductApiClient, ProductApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles(); 
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
