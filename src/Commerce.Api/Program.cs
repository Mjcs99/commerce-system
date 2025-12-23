using Commerce.Application.Interfaces;
using Commerce.Application.Services;
using Commerce.Infrastructure.Repositories;
using Commerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IProductService, ProductService>();


builder.Services.AddDbContext<CommerceDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("CommerceDb")));
builder.Services.AddScoped<IProductRepository, EfProductRepository>();
builder.Services.AddApiVersioning();
// Add controllers
builder.Services.AddControllers();

// Swagger UI via Swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();