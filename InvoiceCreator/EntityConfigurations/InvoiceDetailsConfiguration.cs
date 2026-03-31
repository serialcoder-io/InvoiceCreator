using InvoiceCreator.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


public class InvoiceDetailsConfiguration : IEntityTypeConfiguration<InvoiceDetail>
{
    public void Configure(EntityTypeBuilder<InvoiceDetail> builder)
    {
        builder.HasKey(i => i.InvoiceDetailId);
        builder.Property(i => i.Quantity).IsRequired().HasDefaultValue(1);
        builder.Property(i => i.GrossAmount).HasPrecision(12, 2);
        builder.Property(i => i.NetAmount).HasPrecision(12, 2);
        builder
        .HasOne(i => i.InvoiceMaster)
        .WithMany(i => i.InvoiceDetails)
        .HasForeignKey(i => i.InvoiceMasterId)
        .IsRequired();
        builder
        .HasOne(i => i.Product)
        .WithMany(i => i.InvoiceDetails)
        .HasForeignKey(i => i.ProductId)
        .IsRequired()
         .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(i => new { i.InvoiceMasterId, i.ProductId }).IsUnique();
    }
}