using Bogus;
using DataProvider.DTOs;
using DataProvider.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataDummyProvider.Services
{
    public class ProductService : IProductService
    {
        private static List<ProductDTO> _products = new List<ProductDTO>();
        private readonly ICategoryService _categoryService;
        //private readonly ISupplierService _supplierService;

        public ProductService(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            //_supplierService = supplierService;
        }

        public async Task<List<ProductDTO>> GetProductsAsync()
        {
            if (_products.Count != 0)
                return _products;

            var categories = await _categoryService.GetCategoriesAsync();
            //var suppliers = await _supplierService.GetSuppliersAsync();

            var faker = new Faker<ProductDTO>()
            .RuleFor(u => u.Id, f => f.Random.Number(0, 9999))
            .RuleFor(u => u.Name, f => f.Commerce.Product())
            .RuleFor(u => u.Price, f => Convert.ToDecimal(f.Commerce.Price(10000, 1000000)))
            .RuleFor(u => u.ReleaseDate, f => f.Date.Recent(10))
            .RuleFor(u => u.Active, f => Convert.ToBoolean(f.Random.Number(0, 1)))
            .RuleFor(u => u.CategoryId, f => f.PickRandom(categories).Id);

            _products = faker.Generate(30);

            return _products;
        }

        public Task<ProductDTO> GetProductAsync(int productId)
        {
            var result = _products.FirstOrDefault(x => x.Id == productId);

            return Task.FromResult(result);
        }

        public Task CreateProductAsync(ProductDTO itemToAdd)
        {
            _products.Add(itemToAdd);

            return Task.CompletedTask;
        }

        public Task UpdateProductAsync(ProductDTO itemToUpdate)
        {
            var index = _products.FindIndex(x => x.Id == itemToUpdate.Id);

            if (index != -1)
                _products[index] = itemToUpdate;

            return Task.CompletedTask;
        }

        public Task DeleteProductAsync(ProductDTO itemToDelete)
        {
            var index = _products.FindIndex(x => x.Id == itemToDelete.Id);

            if (index != -1)
                _products.Remove(_products[index]);

            return Task.CompletedTask;
        }

        public async Task<List<ProductDTO>> GetProductsByCategoryAsync(int categoryId)
        {
            List<ProductDTO> loResult = _products;

            if (_products.Count == 0)
                loResult = await GetProductsAsync();

            loResult = loResult.Where(x => x.CategoryId == categoryId).ToList();

            return loResult;
        }

        public async Task<List<ProductDTO>> GetNewProductsAsync()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            //var suppliers = await _supplierService.GetSuppliersAsync();

            var faker = new Faker<ProductDTO>()
            .RuleFor(u => u.Id, f => f.Random.Number(0, 9999))
            .RuleFor(u => u.Name, f => f.Commerce.Product())
            .RuleFor(u => u.Price, f => Convert.ToDecimal(f.Commerce.Price(10000, 1000000)))
            .RuleFor(u => u.ReleaseDate, f => f.Date.Recent(10))
            .RuleFor(u => u.Active, f => Convert.ToBoolean(f.Random.Number(0, 1)))
            .RuleFor(u => u.CategoryId, f => f.PickRandom(categories).Id);

            var loResult = faker.Generate(10);

            return loResult;
        }
    }
}
