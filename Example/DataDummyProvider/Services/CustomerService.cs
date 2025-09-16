using Bogus;
using DataProvider.DTOs;
using DataProvider.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataDummyProvider.Services
{
    public class CustomerService : ICustomerService
    {
        private static List<CustomerDTO> _customer = new List<CustomerDTO>();
        private static readonly List<GenderDTO> _gender = new List<GenderDTO>();

        public Task<List<GenderDTO>> GetGendersAsync()
        {
            if (_gender.Count > 0)
                return Task.FromResult(_gender);

            return Task.FromResult(new List<GenderDTO>
            {
                new GenderDTO { Id = "M", Name = "Male"},
                new GenderDTO { Id = "F", Name = "Female"}
            });
        }

        public Task<List<CustomerDTO>> GetCustomersAsync()
        {
            if (_customer.Count != 0)
                return Task.FromResult(_customer);

            var faker = new Faker<CustomerDTO>()
            .RuleFor(u => u.Id, f => f.Random.AlphaNumeric(5).ToUpper())
            .RuleFor(u => u.CompanyName, f => f.Company.CompanyName())
            .RuleFor(u => u.ContactName, f => f.Name.FirstName())
            .RuleFor(u => u.ContactTitle, f => f.Name.JobTitle())
            .RuleFor(u => u.Address, f => f.Address.FullAddress())
            .RuleFor(u => u.City, f => f.Address.City())
            .RuleFor(u => u.Country, f => f.Address.Country())
            .RuleFor(x => x.GenderId, x => x.PickRandom(new[] { "F", "M" }));

            _customer = faker.Generate(200);

            return Task.FromResult(_customer);
        }

        public Task<CustomerDTO> GetCustomerAsync(string customerId)
        {
            var result = _customer.FirstOrDefault(x => x.Id == customerId);

            return Task.FromResult(result);
        }

        public Task CreateCustomerAsync(CustomerDTO itemToAdd)
        {
            var exist = _customer.Any(x => x.Id == itemToAdd.Id);

            if (exist)
                throw new Exception($"Duplicate customer {itemToAdd.Id}.");

            _customer.Add(itemToAdd);

            return Task.CompletedTask;
        }

        public Task UpdateCustomerAsync(CustomerDTO itemToUpdate)
        {
            var exist = _customer.Any(x => x.Id == itemToUpdate.Id);
            if (!exist)
                throw new Exception($"Customer {itemToUpdate.Id} not exist.");

            var index = _customer.FindIndex(x => x.Id == itemToUpdate.Id);

            if (index != -1)
                _customer[index] = itemToUpdate;

            return Task.CompletedTask;
        }

        public Task DeleteCustomerAsync(CustomerDTO itemToDelete)
        {
            var index = _customer.FindIndex(x => x.Id == itemToDelete.Id);

            if (index != -1)
                _customer.Remove(_customer[index]);

            return Task.CompletedTask;
        }
    }
}
