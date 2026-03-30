namespace InvoiceCreator.Models;

public class Product
{
    public int ProductId { get; set; }

    public required string ProductName { get; set; }

    public required decimal UnitPrice { get; set; } // price per item(tax excluded)

    public required decimal Tax { get; set; } = 15m;

    public ICollection<InvoiceDetail> InvoiceDetails { get; set; } = [];
}