using Commerce.Application.Services;
using Commerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Commerce.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices()
                .AddApplicationServices();

builder.Services.AddDbContext<CommerceDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("CommerceDb")));

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

    await db.Database.EnsureDeletedAsync();
    await db.Database.EnsureCreatedAsync();

    await SeedData.SeedProductsAsync(db, 100);
    await SeedData.SeedProductsAsync(db, count: 100);
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();