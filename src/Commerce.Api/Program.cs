using Commerce.Application.Interfaces;
using Commerce.Infrastructure.Services;
var builder = WebApplication.CreateBuilder(args);
// Register ProductService for IProductService
builder.Services.AddScoped<IProductService, ProductService>();
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