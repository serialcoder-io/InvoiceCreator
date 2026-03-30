using InvoiceCreator.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class InvoiceMasterConfiguration : IEntityTypeConfiguration<InvoiceMaster>
{
    public void Configure(EntityTypeBuilder<InvoiceMaster> builder)
    {
        builder.HasKey(i => i.InvoiceMasterId);
        builder.Property(i => i.CustomerName).IsRequired();
        builder.HasIndex(i => i.CustomerName);
        builder.Property(i => i.GrossAmount).HasPrecision(12, 2);
        builder.Property(i => i.NetAmount).HasPrecision(12, 2);
        builder.Property(i => i.Status).HasConversion<string>().IsRequired();
        builder.HasOne(i => i.CreatedBy)
        .WithMany()
        .HasForeignKey(i => i.CreatedById)
        .IsRequired();
    }
}