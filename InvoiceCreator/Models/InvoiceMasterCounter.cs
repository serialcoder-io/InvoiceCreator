namespace InvoiceCreator.Models
{
    public class InvoiceMasterCounter
    {
        public required string Code { get; set; } = "INV";
        public int LastNumber { get; set; }
    }
}
