using InvoiceCreator.Data;
using InvoiceCreator.Models;
using System.ComponentModel.DataAnnotations;

namespace InvoiceCreator.FormModel
{
    public class InvoiceMasterFormModel
    {

        [Required(ErrorMessage = "Customer is required")]
        public int CustomerId { get; set; }

        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        public InvoiceMasterStatus Status { get; set; }

    }
}
