using Microsoft.EntityFrameworkCore;
using Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Commerce.Infrastructure.Persistence.Configurations;

public sealed class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder.HasKey(ii => ii.ProductId);
        builder.Property(ii => ii.QuantityAvailable).IsRequired();
        builder.HasOne<Product>()
            .WithOne()
            .HasForeignKey<InventoryItem>(ii => ii.ProductId)
            .OnDelete(DeleteBehavior.Cascade);  
    }
}