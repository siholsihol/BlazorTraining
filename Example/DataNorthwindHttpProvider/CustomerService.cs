using DataProvider.Cache;
using DataProvider.Constants;
using DataProvider.DTOs;
using DataProvider.Extensions;
using DataProvider.Services;
using System.Net.Http.Json;

namespace DataNorthwindHttpProvider
{
    public class CustomerService : ICustomerService
    {
        private const string _jsonFile = "sample-data/customers.json";

        private readonly ICacheService _cacheService;
        private readonly HttpClient _httpClient;

        public CustomerService(
            ICacheService cacheService,
            HttpClient httpClient)
        {
            _cacheService = cacheService;
            _httpClient = httpClient;
        }

        public async Task<List<CustomerDTO>> GetCustomersAsync()
        {
            var customers = await _cacheService.GetOrSetAsync(
                CacheConstant.AllCustomer,
                GetCustomers);

            return customers ?? Enumerable.Empty<CustomerDTO>().ToList();
        }

        private async Task<List<CustomerDTO>> GetCustomers()
        {
            var result = await _httpClient.GetFromJsonAsync<List<CustomerDTO>>(_jsonFile);

            return result ?? Enumerable.Empty<CustomerDTO>().ToList();
        }

        public async Task<CustomerDTO?> GetCustomerAsync(string customerId)
        {
            var customers = await _cacheService.GetAsync<List<CustomerDTO>>(CacheConstant.AllCustomer);
            var result = customers?.FirstOrDefault(x => x.Id == customerId);

            return result;
        }

        public async Task CreateCustomerAsync(CustomerDTO itemToAdd)
        {
            var customers = await _cacheService.GetAsync<List<CustomerDTO>>(CacheConstant.AllCustomer);
            if (customers == null)
                throw new NullReferenceException();

            customers.Add(itemToAdd);

            await _cacheService.SetAsync(CacheConstant.AllCustomer, customers);
        }

        public async Task UpdateCustomerAsync(CustomerDTO itemToUpdate)
        {
            var customers = await _cacheService.GetAsync<List<CustomerDTO>>(CacheConstant.AllCustomer);
            if (customers == null)
                throw new NullReferenceException();

            var index = customers.FindIndex(x => x.Id == itemToUpdate.Id);

            if (index != -1)
            {
                customers[index] = itemToUpdate;

                await _cacheService.SetAsync(CacheConstant.AllCustomer, customers);
            }
        }

        public async Task DeleteCustomerAsync(CustomerDTO itemToDelete)
        {
            var customers = await _cacheService.GetAsync<List<CustomerDTO>>(CacheConstant.AllCustomer);
            if (customers == null)
                throw new NullReferenceException();

            var index = customers.FindIndex(x => x.Id == itemToDelete.Id);

            if (index != -1)
            {
                customers.Remove(customers[index]);

                await _cacheService.SetAsync(CacheConstant.AllCustomer, customers);
            }
        }
    }
}
