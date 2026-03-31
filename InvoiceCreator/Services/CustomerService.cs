using InvoiceCreator.Data;
using InvoiceCreator.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceCreator.Services
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllCustomersAsync();
        Task<Customer> AddCustomerAsync(Customer customer);
        Task<Customer> UpdateCustomerAsync(Customer customer);
    }

    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers
                .AsNoTracking()
                .OrderBy(c => c.CustomerName)
                .ToListAsync();
        }

        public async Task<Customer> AddCustomerAsync(Customer customer)
        {
            if (customer == null || string.IsNullOrWhiteSpace(customer.CustomerName))
            {
                throw new ArgumentException("Customer name is required.");
            }

            var entity = new Customer
            {
                CustomerName = customer.CustomerName.Trim()
            };

            _context.Customers.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Customer> UpdateCustomerAsync(Customer customer)
        {
            if (customer == null || string.IsNullOrWhiteSpace(customer.CustomerName))
            {
                throw new ArgumentException("Customer name is required.");
            }

            var existing = await _context.Customers.FindAsync(customer.CustomerId) ?? throw new InvalidOperationException($"Customer {customer.CustomerId} not found.");
            existing.CustomerName = customer.CustomerName.Trim();

            await _context.SaveChangesAsync();

            return existing;
        }
    }
}