namespace InvoiceCreator.Models
{
    public class InvoiceDetailCounter
    {
        public required string Code { get; set; } = "INVD";
        public int LastNumber { get; set; }
    }
}
