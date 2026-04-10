using ApiEstoque.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstoque.Mapping
{
    public class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure (EntityTypeBuilder<Product>builder)
        {
            builder.ToTable("Products");
            builder.HasKey(p => p.Id);
            builder.HasIndex(p => p.Sku).IsUnique();
            builder.Property(p => p.Sku).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).HasMaxLength(500).IsRequired(false);
            builder.Property(p => p.CategoryId).IsRequired();
            builder.Property(p => p.IsActive).IsRequired();
            builder.Property(p => p.MinStock).IsRequired();
            builder.Property(p => p.CreatedAt).IsRequired().HasColumnType("datetime2");
            builder.Property(p => p.UpdatedAt).HasColumnType("datetime2");
        }
    }
}
