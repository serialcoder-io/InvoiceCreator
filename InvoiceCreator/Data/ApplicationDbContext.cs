using InvoiceCreator.EntityConfigurations;
using InvoiceCreator.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InvoiceCreator.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<InvoiceMaster> InvoiceMasters { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<InvoiceMasterCounter> InvoiceMasterCounter { get; set; }
        public DbSet<InvoiceDetailCounter> InvoiceDetailCounter{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new InvoiceMasterConfiguration());
            modelBuilder.ApplyConfiguration(new InvoiceDetailsConfiguration());
            modelBuilder.ApplyConfiguration(new InvoiceMasterCounterConfiguration());
            modelBuilder.ApplyConfiguration(new InvoiceDetailCounterConfiguration());
        }
    }
}
