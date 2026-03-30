namespace InvoiceCreator.Models
{
    public class Customer
    {

        public int CustomerId { get; set; }

        public required string CustomerName { get; set; }

        public ICollection<InvoiceMaster> InvoiceMasters { get; set; } = [];
    }
}

