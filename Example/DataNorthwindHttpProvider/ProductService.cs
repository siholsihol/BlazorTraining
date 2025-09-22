using DataProvider.Cache;
using DataProvider.Constants;
using DataProvider.DTOs;
using DataProvider.Extensions;
using DataProvider.Services;
using System.Net.Http.Json;

namespace DataNorthwindHttpProvider
{
    public class ProductService : IProductService
    {
        private const string _jsonFile = "sample-data/products.json";

        private readonly ICategoryService _categoryService;
        private readonly ICacheService _cacheService;
        private readonly ISupplierService _supplierService;
        private readonly HttpClient _httpClient;

        public ProductService(
            ICategoryService categoryService,
            ICacheService cacheService,
            ISupplierService supplierService,
            HttpClient httpClient)
        {
            _categoryService = categoryService;
            _cacheService = cacheService;
            _supplierService = supplierService;
            _httpClient = httpClient;
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
            var result = await _httpClient.GetFromJsonAsync<List<ProductDTO>>(_jsonFile);

            return result ?? Enumerable.Empty<ProductDTO>().ToList();
        }

        public async Task<ProductDTO?> GetProductAsync(int productId)
        {
            var products = await _cacheService.GetAsync<List<ProductDTO>>(CacheConstant.AllProduct);
            var result = products?.FirstOrDefault(x => x.Id == productId);

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
