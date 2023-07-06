using Bogus;
using DataDummyProvider.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataDummyProvider.Services
{
    public static class CustomerService
    {
        private static List<CustomerDTO> _customer = new List<CustomerDTO>();
        private static readonly List<GenderDTO> _gender = new List<GenderDTO>();

        public static List<GenderDTO> GetGenders()
        {
            if (_gender.Count > 0)
                return _gender;

            return new List<GenderDTO>
            {
                new GenderDTO { Id = "M", Name = "Male"},
                new GenderDTO { Id = "F", Name = "Female"}
            };
        }

        public static List<CustomerDTO> GetCustomers()
        {
            if (_customer.Count != 0)
                return _customer;

            var faker = new Faker<CustomerDTO>()
            .RuleFor(u => u.Id, f => f.Random.AlphaNumeric(5).ToUpper())
            .RuleFor(u => u.CompanyName, f => f.Company.CompanyName())
            .RuleFor(u => u.ContactName, f => f.Name.FirstName())
            .RuleFor(u => u.ContactTitle, f => f.Name.JobTitle())
            .RuleFor(u => u.Address, f => f.Address.FullAddress())
            .RuleFor(u => u.City, f => f.Address.City())
            .RuleFor(u => u.Country, f => f.Address.Country())
            .RuleFor(x => x.GenderId, x => x.PickRandom(new[] { "F", "M" }));

            _customer = faker.Generate(30);

            return _customer;
        }

        public static CustomerDTO GetCustomer(string customerId)
        {
            return _customer.FirstOrDefault(x => x.Id == customerId);
        }

        public static void CreateCustomer(CustomerDTO itemToAdd)
        {
            var exist = _customer.Any(x => x.Id == itemToAdd.Id);

            if (exist)
                throw new Exception($"Duplicate customer {itemToAdd.Id}.");

            _customer.Add(itemToAdd);
        }

        public static void UpdateCustomer(CustomerDTO itemToUpdate)
        {
            var exist = _customer.Any(x => x.Id == itemToUpdate.Id);
            if (!exist)
                throw new Exception($"Customer {itemToUpdate.Id} not exist.");

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
