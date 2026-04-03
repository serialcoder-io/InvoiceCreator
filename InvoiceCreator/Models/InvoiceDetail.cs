namespace InvoiceCreator.Models
{
    public class InvoiceDetail
    {
        public int InvoiceDetailId { get; set; }

        public int InvoiceMasterId { get; set; }

        public InvoiceMaster? InvoiceMaster { get; set; }

        public int ProductId { get; set; }

        public Product? Product { get; set; }

        public decimal UnitPrice { get; set; }

        public required int Quantity { get; set; } = 1;

        public decimal GrossAmount { get; set; } // total price (tax excluded)

        public required decimal NetAmount { get; set; } // total price (with tax)
    }
}

