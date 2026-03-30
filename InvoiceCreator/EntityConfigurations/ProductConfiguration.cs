using InvoiceCreator.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.ProductId);
        builder.Property(p => p.ProductName).HasMaxLength(100).IsRequired();
        builder.HasIndex(p => p.ProductName).IsUnique();
        builder.Property(P => P.UnitPrice).HasPrecision(12, 2).IsRequired();
        builder.Property(P => P.Tax).HasPrecision(12, 2).HasDefaultValue(0m).IsRequired();
    }
}
