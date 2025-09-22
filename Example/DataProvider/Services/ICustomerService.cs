using DataProvider.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataProvider.Services
{
    public interface ICustomerService
    {
        public Task CreateCustomerAsync(CustomerDTO itemToAdd);
        public Task DeleteCustomerAsync(CustomerDTO itemToDelete);
        public Task UpdateCustomerAsync(CustomerDTO itemToUpdate);
        public Task<CustomerDTO?> GetCustomerAsync(string customerId);
        public Task<List<CustomerDTO>> GetCustomersAsync();
    }
}