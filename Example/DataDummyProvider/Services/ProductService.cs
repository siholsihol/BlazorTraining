using Bogus;
using DataDummyProvider.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataDummyProvider.Services
{
    public static class ProductService
    {
        private static List<ProductDTO> _products = new List<ProductDTO>();

        public static List<ProductDTO> GetProducts()
        {
            if (_products.Count != 0)
                return _products;

            var categories = CategoryService.GetCategories();
            var suppliers = SupplierService.GetSuppliers();

            var faker = new Faker<ProductDTO>()
            .RuleFor(u => u.Id, f => f.Random.Number(0, 9999))
            .RuleFor(u => u.Name, f => f.Commerce.Product())
            .RuleFor(u => u.Price, f => Convert.ToDecimal(f.Commerce.Price(10000, 1000000)))
            .RuleFor(u => u.ReleaseDate, f => f.Date.Recent(10))
            .RuleFor(u => u.Active, f => Convert.ToBoolean(f.Random.Number(0, 1)))
            .RuleFor(u => u.CategoryId, f => f.PickRandom(categories).Id)
            .RuleFor(u => u.SupplierId, f => f.PickRandom(suppliers).Id);

            _products = faker.Generate(30);

            return _products;
        }

        public static ProductDTO GetProduct(int productId)
        {
            return _products.FirstOrDefault(x => x.Id == productId);
        }

        public static void CreateProduct(ProductDTO itemToAdd)
        {
            _products.Add(itemToAdd);
        }

        public static void UpdateProduct(ProductDTO itemToUpdate)
        {
            var index = _products.FindIndex(x => x.Id == itemToUpdate.Id);

            if (index != -1)
                _products[index] = itemToUpdate;
        }

        public static void DeleteProduct(ProductDTO itemToDelete)
        {
            var index = _products.FindIndex(x => x.Id == itemToDelete.Id);

            if (index != -1)
                _products.Remove(_products[index]);
        }

        public static List<ProductDTO> GetProductsByCategory(int categoryId)
        {
            List<ProductDTO> loResult = _products;

            if (_products.Count == 0)
                loResult = GetProducts();

            loResult = loResult.Where(x => x.CategoryId == categoryId).ToList();

            return loResult;
        }

        public static List<ProductDTO> GetNewProducts()
        {
            var categories = CategoryService.GetCategories();
            var suppliers = SupplierService.GetSuppliers();

            var faker = new Faker<ProductDTO>()
            .RuleFor(u => u.Id, f => f.Random.Number(0, 9999))
            .RuleFor(u => u.Name, f => f.Commerce.Product())
            .RuleFor(u => u.Price, f => Convert.ToDecimal(f.Commerce.Price(10000, 1000000)))
            .RuleFor(u => u.ReleaseDate, f => f.Date.Recent(10))
            .RuleFor(u => u.Active, f => Convert.ToBoolean(f.Random.Number(0, 1)))
            .RuleFor(u => u.CategoryId, f => f.PickRandom(categories).Id)
            .RuleFor(u => u.SupplierId, f => f.PickRandom(suppliers).Id);

            var loResult = faker.Generate(10);

            return loResult;
        }
    }
}
