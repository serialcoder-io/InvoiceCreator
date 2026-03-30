using InvoiceCreator.Data;

namespace InvoiceCreator.Models
{
    public enum InvoiceMasterStatus
    {
        Pending,
        Completed,
    }


    public class InvoiceMaster
    {
        public int InvoiceMasterId { get; set; }

        public required string CustomerName { get; set; }

        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }

        public decimal GrossAmount { get; set; } // price (with tax)

        public decimal NetAmount { get; set; } // price (tax exluded)

        public DateTime CreatedAt { get; set; }

        public string CreatedById { get; set; } = null!;

        public ApplicationUser CreatedBy { get; set; } = null!;

        public required InvoiceMasterStatus Status { get; set; } = InvoiceMasterStatus.Pending;

        public ICollection<InvoiceDetail> InvoiceDetails { get; set; } = [];
    }
}

