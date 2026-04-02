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

        // get all
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

        // Get by id
        public async Task<InvoiceMaster?> GetInvoiceMasterByIdAsync(int id)
        {
            return await _context.InvoiceMasters
            .Include(i => i.InvoiceDetails)
            .ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(i => i.InvoiceMasterId == id);
        }

        // Add new invoice master
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

        // Update existing invoice master
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
                existingMaster.NetAmount = master.NetAmount;
                existingMaster.GrossAmount = master.GrossAmount;

                // Delete
                if (deletedDetails.Count != 0)
                {
                    var toDelete = await _context.InvoiceDetails
                        .Where(d => deletedDetails.Contains(d.InvoiceDetailId))
                        .ToListAsync();

                    _context.InvoiceDetails.RemoveRange(toDelete);
                }

                // Update
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

                // Insert
                if (newDetails.Count != 0)
                {
                    foreach (var detail in newDetails)
                    {
                        detail.InvoiceMasterId = existingMaster.InvoiceMasterId;
                    }

                    await _context.InvoiceDetails.AddRangeAsync(newDetails);
                }

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