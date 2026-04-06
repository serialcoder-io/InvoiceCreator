using InvoiceCreator.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceCreator.EntityConfigurations
{
    public class InvoiceMasterCounterConfiguration : IEntityTypeConfiguration<InvoiceMasterCounter>
    {
        public void Configure(EntityTypeBuilder<InvoiceMasterCounter> builder)
        {
            builder.HasKey(i => i.Code);
        }
    }
}
