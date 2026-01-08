using Microsoft.EntityFrameworkCore;
using Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Commerce.Infrastructure.Persistence.Configurations;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.ExternalUserId).IsRequired().HasMaxLength(100);
        builder.HasIndex(c => c.ExternalUserId).IsUnique();
        builder.Property(c => c.FirstName).HasMaxLength(50);
        builder.Property(c => c.LastName).HasMaxLength(50);
        builder.Property(c => c.Email).IsRequired().HasMaxLength(256);
        builder.HasIndex(c => c.Email).IsUnique();
        builder.Property(c => c.CreatedAtUtc).IsRequired(); 
    }
}