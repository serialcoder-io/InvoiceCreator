using InvoiceCreator.Data;
using InvoiceCreator.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceCreator.Services
{
    public interface IInvoiceMasterService
    {
        Task<List<InvoiceMaster>> GetAllInvoiceMastersAsync();
        Task<InvoiceMaster?> GetInvoiceMasterByIdAsync(int id);

        Task<InvoiceMaster> AddInvoiceMasterAsync(
            InvoiceMaster master,
            List<InvoiceDetail> details);

        Task UpdateInvoiceMasterAsync(
            InvoiceMaster master,
            List<InvoiceDetail> newDetails,
            List<InvoiceDetail> updatedDetails,
            List<int> deletedDetails
        );
    }

    public class InvoiceMasterService : IInvoiceMasterService
    {
        private readonly ApplicationDbContext _context;

        public InvoiceMasterService(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🔹 GET ALL
        public async Task<List<InvoiceMaster>> GetAllInvoiceMastersAsync()
        {
            return await _context.InvoiceMasters
                .AsNoTracking()
                .Select(im => new InvoiceMaster
                {
                    InvoiceMasterId = im.InvoiceMasterId,
                    CustomerId = im.CustomerId,
                    CustomerName = im.CustomerName,
                    GrossAmount = im.GrossAmount,
                    NetAmount = im.NetAmount,
                    CreatedAt = im.CreatedAt,
                    Status = im.Status
                })
                .ToListAsync();
        }

        // 🔹 GET BY ID
        public async Task<InvoiceMaster?> GetInvoiceMasterByIdAsync(int id)
        {
            return await _context.InvoiceMasters
            .Include(i => i.InvoiceDetails)
            .ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(i => i.InvoiceMasterId == id);
                }

        // ADD new invoice master
        public async Task<InvoiceMaster> AddInvoiceMasterAsync(
            InvoiceMaster master,
            List<InvoiceDetail> details)
        {
            master.InvoiceDetails = details;

            master.NetAmount = details.Sum(d => d.NetAmount);
            master.GrossAmount = details.Sum(d => d.GrossAmount);

            _context.InvoiceMasters.Add(master);

            await _context.SaveChangesAsync();

            return master;
        }

        // UPDATE existing invoice master
        public async Task UpdateInvoiceMasterAsync(
            InvoiceMaster master,
            List<InvoiceDetail> newDetails,
            List<InvoiceDetail> updatedDetails,
            List<int> deletedDetails)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // load 
                var existingMaster = await _context.InvoiceMasters
                    .Include(i => i.InvoiceDetails)
                    .FirstOrDefaultAsync(i => i.InvoiceMasterId == master.InvoiceMasterId);

                if (existingMaster == null)
                    throw new Exception("Invoice not found");

                // Update master
                existingMaster.CustomerId = master.CustomerId;
                existingMaster.CustomerName = master.CustomerName;
                existingMaster.Status = master.Status;

                // DELETE
                if (deletedDetails.Any())
                {
                    var toDelete = await _context.InvoiceDetails
                        .Where(d => deletedDetails.Contains(d.InvoiceDetailId))
                        .ToListAsync();

                    _context.InvoiceDetails.RemoveRange(toDelete);
                }

                // UPDATE
                foreach (var detail in updatedDetails)
                {
                    var existingDetail = await _context.InvoiceDetails
                        .FirstOrDefaultAsync(d => d.InvoiceDetailId == detail.InvoiceDetailId);

                    if (existingDetail != null)
                    {
                        existingDetail.ProductId = detail.ProductId;
                        existingDetail.Quantity = detail.Quantity;
                        existingDetail.NetAmount = detail.NetAmount;
                        existingDetail.GrossAmount = detail.GrossAmount;
                    }
                }

                // INSERT
                if (newDetails.Any())
                {
                    foreach (var detail in newDetails)
                    {
                        detail.InvoiceMasterId = existingMaster.InvoiceMasterId;
                    }

                    await _context.InvoiceDetails.AddRangeAsync(newDetails);
                }

                // Compute amounts
                var allDetails = await _context.InvoiceDetails
                    .Where(d => d.InvoiceMasterId == existingMaster.InvoiceMasterId)
                    .ToListAsync();

                existingMaster.NetAmount = allDetails.Sum(d => d.NetAmount);
                existingMaster.GrossAmount = allDetails.Sum(d => d.GrossAmount);

                // Save
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}