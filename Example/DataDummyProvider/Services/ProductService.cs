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
    public class ProductService : IProductService
    {
        private readonly ICategoryService _categoryService;
        private readonly ICacheService _cacheService;
        private readonly ISupplierService _supplierService;

        public ProductService(
            ICategoryService categoryService,
            ICacheService cacheService,
            ISupplierService supplierService)
        {
            _categoryService = categoryService;
            _cacheService = cacheService;
            _supplierService = supplierService;
        }

        public async Task<List<ProductDTO>> GetProductsAsync()
        {
            var products = await _cacheService.GetOrSetAsync(
                CacheConstant.AllProduct,
                GetProducts);

            return products ?? Enumerable.Empty<ProductDTO>().ToList();
        }

        private async Task<List<ProductDTO>> GetProducts()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            var suppliers = await _supplierService.GetSuppliersAsync();

            var startId = 1;

            var faker = new Faker<ProductDTO>()
            .RuleFor(u => u.Id, f => startId++)
            .RuleFor(u => u.Name, f => f.Commerce.ProductName())
            .RuleFor(u => u.Price, f => Convert.ToDecimal(f.Commerce.Price(10000, 1000000)))
            .RuleFor(u => u.Discontinued, f => Convert.ToBoolean(f.Random.Number(0, 1)))
            .RuleFor(u => u.CategoryId, f => f.PickRandom(categories).Id)
            .RuleFor(u => u.SupplierId, f => f.PickRandom(suppliers).Id);

            var products = faker.Generate(30);

            return products;
        }

        public async Task<ProductDTO?> GetProductAsync(int productId)
        {
            var products = await _cacheService.GetAsync<List<ProductDTO>>(CacheConstant.AllProduct);
            var result = products.FirstOrDefault(x => x.Id == productId);

            return result;
        }

        public async Task CreateProductAsync(ProductDTO itemToAdd)
        {
            var products = await _cacheService.GetAsync<List<ProductDTO>>(CacheConstant.AllProduct);
            if (products == null)
                throw new NullReferenceException();

            itemToAdd.Id = products.Count;
            products.Add(itemToAdd);

            await _cacheService.SetAsync(CacheConstant.AllProduct, products);
        }

        public async Task UpdateProductAsync(ProductDTO itemToUpdate)
        {
            var products = await _cacheService.GetAsync<List<ProductDTO>>(CacheConstant.AllProduct);
            if (products == null)
                throw new NullReferenceException();

            var index = products.FindIndex(x => x.Id == itemToUpdate.Id);

            if (index != -1)
            {
                products[index] = itemToUpdate;

                await _cacheService.SetAsync(CacheConstant.AllProduct, products);
            }
        }

        public async Task DeleteProductAsync(ProductDTO itemToDelete)
        {
            var products = await _cacheService.GetAsync<List<ProductDTO>>(CacheConstant.AllProduct);
            if (products == null)
                throw new NullReferenceException();

            var index = products.FindIndex(x => x.Id == itemToDelete.Id);

            if (index != -1)
            {
                products.Remove(products[index]);

                await _cacheService.SetAsync(CacheConstant.AllProduct, products);
            }
        }

        public async Task<List<ProductDTO>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _cacheService.GetAsync<List<ProductDTO>>(CacheConstant.AllProduct);

            var loResult = products.Where(x => x.CategoryId == categoryId).ToList();

            return loResult;
        }
    }
}
