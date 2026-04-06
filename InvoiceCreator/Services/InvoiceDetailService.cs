using InvoiceCreator.Data;
using InvoiceCreator.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceCreator.Services
{
    public interface IInvoiceDetailService
    {
        Task<InvoiceDetail> AddInvoiceDetailAsync(InvoiceDetail detail);
        Task<List<InvoiceDetail>> GetByInvoiceIdAsync(string invoiceMasterId);
    }

    public class InvoiceDetailService : IInvoiceDetailService
    {
        private readonly ApplicationDbContext _context;

        public InvoiceDetailService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<InvoiceDetail> AddInvoiceDetailAsync(InvoiceDetail detail)
        {
            // retrieve products to compute amounts
            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductId == detail.ProductId);

            if (product == null)
                throw new Exception("Product not found");

            var gross = product.UnitPrice * detail.Quantity;
            var net = gross * (1 + product.Tax / 100m);

            detail.NetAmount = net;
            detail.GrossAmount = gross;

            _context.InvoiceDetails.Add(detail);

            await _context.SaveChangesAsync();

            return detail;
        }

        public async Task<List<InvoiceDetail>> GetByInvoiceIdAsync(string invoiceMasterId)
        {
            return await _context.InvoiceDetails
                .Where(d => d.InvoiceMasterId == invoiceMasterId)
                .Include(d => d.Product)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}