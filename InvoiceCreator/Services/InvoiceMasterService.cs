using InvoiceCreator.Data;
using InvoiceCreator.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceCreator.Services
{
    public interface IInvoiceMasterService
    {
        Task<List<InvoiceMaster>> GetAllInvoiceMastersAsync();
        Task<InvoiceMaster> AddInvoiceMasterAsync(InvoiceMaster invoice);
    }
    public class InvoiceMasterService : IInvoiceMasterService
    {
        private readonly ApplicationDbContext _context;

        public InvoiceMasterService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<InvoiceMaster>> GetAllInvoiceMastersAsync()
        {
            return await _context.InvoiceMasters
                .AsNoTracking()
                .Select(im => new InvoiceMaster
                {
                    InvoiceMasterId = im.InvoiceMasterId,
                    CustomerName = im.CustomerName,
                    GrossAmount = im.GrossAmount,
                    NetAmount = im.NetAmount,
                    CreatedAt = im.CreatedAt,
                    Status = im.Status
                })
                .ToListAsync();
        }

        public async Task<InvoiceMaster> AddInvoiceMasterAsync(InvoiceMaster invoice)
        {
            invoice.CreatedAt = DateTime.UtcNow;

            _context.InvoiceMasters.Add(invoice);
            await _context.SaveChangesAsync();

            return invoice;
        }
    }
}
