using InvoiceCreator.Data;
using InvoiceCreator.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceCreator.Services
{
    public interface IInvoiceMasterService
    {
        Task<List<InvoiceMaster>> GetAllInvoiceMastersAsync();
        Task<InvoiceMaster> AddInvoiceMasterAsync(InvoiceMaster master, List<InvoiceDetail> details);
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

        public async Task<InvoiceMaster> AddInvoiceMasterAsync(InvoiceMaster master, List<InvoiceDetail> details)
        {
            // Lier les détails
            master.InvoiceDetails = details;

            // Calcul des totaux
            master.NetAmount = details.Sum(d => d.NetAmount);
            master.GrossAmount = details.Sum(d => d.GrossAmount);

            // Ajouter
            _context.InvoiceMasters.Add(master);

            await _context.SaveChangesAsync();

            return master; // 🔥 utile pour récupérer l'ID
        }
    }
}
