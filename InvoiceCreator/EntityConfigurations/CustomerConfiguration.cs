using InvoiceCreator.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.CustomerId);
        builder.Property(c => c.CustomerName).IsRequired();
        builder.Property(c => c.CustomerName).HasMaxLength(50);
    }
}