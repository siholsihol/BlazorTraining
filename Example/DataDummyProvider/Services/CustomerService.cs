using Bogus;
using DataProvider.Cache;
using DataProvider.Constants;
using DataProvider.DTOs;
using DataProvider.Extensions;
using DataProvider.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataDummyProvider.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICacheService _cacheService;

        public CustomerService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<List<CustomerDTO>> GetCustomersAsync()
        {
            var customers = await _cacheService.GetOrSetAsync(
                CacheConstant.AllCustomer,
                GetCustomers);

            return customers ?? Enumerable.Empty<CustomerDTO>().ToList();
        }

        private Task<List<CustomerDTO>> GetCustomers()
        {
            var faker = new Faker<CustomerDTO>()
            .RuleFor(u => u.Id, f => f.Random.AlphaNumeric(5).ToUpper())
            .RuleFor(u => u.CompanyName, f => f.Company.CompanyName())
            .RuleFor(u => u.ContactName, f => f.Name.FirstName())
            .RuleFor(u => u.ContactTitle, f => f.Name.JobTitle())
            .RuleFor(u => u.Address, f => f.Address.FullAddress())
            .RuleFor(u => u.City, f => f.Address.City())
            .RuleFor(u => u.Country, f => f.Address.Country());

            var result = faker.Generate(100);

            return Task.FromResult(result);
        }

        public async Task<CustomerDTO?> GetCustomerAsync(string customerId)
        {
            var customers = await _cacheService.GetAsync<List<CustomerDTO>>(CacheConstant.AllCustomer);
            var result = customers.FirstOrDefault(x => x.Id == customerId);

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
