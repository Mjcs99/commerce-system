using Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Infrastructure.Persistence;

public class CommerceDbContext : DbContext
{
    public CommerceDbContext(DbContextOptions<CommerceDbContext> options) : base(options) {}

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(b =>
        {
            b.HasKey(p => p.Id);
            b.Property(p => p.Sku).IsRequired().HasMaxLength(64);
            b.HasIndex(p => p.Sku).IsUnique();

            b.Property(p => p.Name).IsRequired().HasMaxLength(200);
            b.Property(p => p.PriceAmount).IsRequired();
        });

        
    }
}
