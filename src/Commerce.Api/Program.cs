using Commerce.Application.Services;
using Commerce.Application.Services.Outbox;
using Commerce.Infrastructure.Persistence;
using Commerce.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi;
using Commerce.Api.Outbox;
using Commerce.Application.Interfaces.In.Outbox;
using Commerce.Api.Messaging;
using Commerce.Application.Interfaces.In;
using Commerce.Application.Handlers;
// Come clean up
var builder = WebApplication.CreateBuilder(args);
Console.WriteLine($"ENV = {builder.Environment.EnvironmentName}");
Console.WriteLine($"Swagger:ClientId = {builder.Configuration["Swagger:ClientId"]}");
builder.Services.AddScoped<IOutboxPublisher, OutboxPublisher>();
builder.Services.AddHostedService<EmailConsumerHostedService>();
builder.Services.AddHostedService<OrdersConsumerHostedService>();
builder.Services.AddHostedService<OutboxPublisherHostedService>();
builder.Services.AddInfrastructureServices(builder.Configuration)
                .AddApplicationServices();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddScoped<IIntegrationEventHandler, OrderPlacedEventHandler>();
builder.Services.AddScoped<IIntegrationEventHandler, OrderProcessedEmailHandler>();
builder.Services.AddAuthorization();
builder.Services.AddApiVersioning();

builder.Services.AddControllers();

// Swagger UI via Swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Commerce API", Version = "v1" });

    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"https://commercecustomers.ciamlogin.com/009fb04d-a59f-4035-9375-4448a9b6c727/oauth2/v2.0/authorize"),
                TokenUrl         = new Uri($"https://commercecustomers.ciamlogin.com/009fb04d-a59f-4035-9375-4448a9b6c727/oauth2/v2.0/token"),
                Scopes = new Dictionary<string, string>
                {
                    { $"api://c4879e2e-8e97-4001-bd8d-4e7fea53c27a/access_as_user", "Access Commerce API as user" }
                }
            }
        },
       
    });

    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("oauth2", document)] =
            new List<string>
            {
                "api://c4879e2e-8e97-4001-bd8d-4e7fea53c27a/access_as_user"
            }
    });

});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<CommerceDbContext>();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Commerce API v1");

        c.OAuthClientId(builder.Configuration["Swagger:ClientId"]);
        c.OAuthAppName("Commerce Swagger UI");
        c.OAuthUsePkce();

        c.OAuthScopes("api://c4879e2e-8e97-4001-bd8d-4e7fea53c27a/access_as_user");

        c.OAuthScopeSeparator(" ");
    });
    
    await SeedData.SeedProductsAsync(db, count: 10);
    
}
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();