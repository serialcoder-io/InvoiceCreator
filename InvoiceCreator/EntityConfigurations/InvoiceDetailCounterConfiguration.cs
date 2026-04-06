using InvoiceCreator.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceCreator.EntityConfigurations
{
    public class InvoiceDetailCounterConfiguration : IEntityTypeConfiguration<InvoiceDetailCounter>
    {
        public void Configure(EntityTypeBuilder<InvoiceDetailCounter> builder)
        {
            builder.HasKey(i => i.Code);
        }
    }
}
