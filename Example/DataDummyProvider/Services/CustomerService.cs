using Bogus;
using DataDummyProvider.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace DataDummyProvider.Services
{
    public static class CustomerService
    {
        private static List<CustomerDTO> _customer = new List<CustomerDTO>();

        public static List<CustomerDTO> GenerateCustomer(int count)
        {
            if (_customer.Count != 0)
                return GetCustomers();

            var faker = new Faker<CustomerDTO>()
            .RuleFor(u => u.Id, f => f.Random.AlphaNumeric(5).ToUpper())
            .RuleFor(u => u.CompanyName, f => f.Company.CompanyName())
            .RuleFor(u => u.ContactName, f => f.Name.FirstName())
            .RuleFor(u => u.ContactTitle, f => f.Name.JobTitle())
            .RuleFor(u => u.Address, f => f.Address.FullAddress())
            .RuleFor(u => u.City, f => f.Address.City())
            .RuleFor(u => u.Country, f => f.Address.Country());

            _customer = faker.Generate(count);

            return GetCustomers();
        }

        public static List<CustomerDTO> GetCustomers()
        {
            return _customer;
        }

        public static CustomerDTO GetCustomer(string customerId)
        {
            return _customer.FirstOrDefault(x => x.Id == customerId);
        }

        public static void CreateCustomer(CustomerDTO itemToAdd)
        {
            _customer.Add(itemToAdd);
        }

        public static void UpdateCustomer(CustomerDTO itemToUpdate)
        {
            var index = _customer.FindIndex(x => x.Id == itemToUpdate.Id);

            if (index != -1)
                _customer[index] = itemToUpdate;
        }

        public static void DeleteCustomer(CustomerDTO itemToDelete)
        {
            var index = _customer.FindIndex(x => x.Id == itemToDelete.Id);

            if (index != -1)
                _customer.Remove(_customer[index]);
        }
    }
}
