using Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Infrastructure.Persistence;

public class CommerceDbContext : DbContext
{
    public CommerceDbContext(DbContextOptions<CommerceDbContext> options) : base(options) { }
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Category => Set<Category>();
    public DbSet<Customer> Customer => Set<Customer>();
    public DbSet<Order> Order => Set<Order>();
    public DbSet<InventoryItem> InventoryItem => Set<InventoryItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CommerceDbContext).Assembly);
    }
}

