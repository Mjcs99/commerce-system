using Commerce.Application.Services;
using Commerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Commerce.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration)
                .AddApplicationServices();

builder.Services.AddApiVersioning();

builder.Services.AddControllers();

// Swagger UI via Swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<CommerceDbContext>();


    await SeedData.SeedProductsAsync(db, count: 10);
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();