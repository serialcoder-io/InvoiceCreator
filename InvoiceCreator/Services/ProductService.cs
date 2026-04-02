using InvoiceCreator.Components.Pages;
using InvoiceCreator.Data;
using InvoiceCreator.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceCreator.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> AddProductAsync(Product product);
        Task<Product?> FindProductByNameAsync(string productName);
    }

    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .OrderBy(p => p.ProductName)
                .ToListAsync();
        }

        public async Task<Product?> FindProductByNameAsync(string productName)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.ProductName == productName);
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            ArgumentNullException.ThrowIfNull(product);

            if (string.IsNullOrWhiteSpace(product.ProductName))
            {
                throw new ArgumentException("Product name cannot be null or empty");
            }

            var productName = product.ProductName.Trim();

            var existingProduct = await FindProductByNameAsync(productName);

            if (existingProduct != null)
            {
                throw new Exception("Product already exists");
            }

            var entity = new Product
            {
                ProductName = productName,
                UnitPrice = product.UnitPrice,
                Tax = product.Tax,
            };

            _context.Products.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}