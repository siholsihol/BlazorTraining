using DataProvider.Cache;
using DataProvider.Constants;
using DataProvider.DTOs;
using DataProvider.Extensions;
using DataProvider.Services;
using System.Net.Http.Json;

namespace DataNorthwindHttpProvider
{
    public class CategoryService : ICategoryService
    {
        private const string _jsonFile = "sample-data/categories.json";

        private readonly ICacheService _cacheService;
        private readonly HttpClient _httpClient;

        public CategoryService(
            ICacheService cacheService,
            HttpClient httpClient)
        {
            _cacheService = cacheService;
            _httpClient = httpClient;
        }

        public async Task<List<CategoryDTO>> GetCategoriesAsync()
        {
            var categories = await _cacheService.GetOrSetAsync(
                CacheConstant.AllCategory,
                GetCategories);

            return categories ?? Enumerable.Empty<CategoryDTO>().ToList();
        }

        private async Task<List<CategoryDTO>> GetCategories()
        {
            var result = await _httpClient.GetFromJsonAsync<List<CategoryDTO>>(_jsonFile);

            return result ?? Enumerable.Empty<CategoryDTO>().ToList();
        }

        public async Task<CategoryDTO?> GetCategoryAsync(int categoryId)
        {
            var categories = await _cacheService.GetAsync<List<CategoryDTO>>(CacheConstant.AllCategory);
            var result = categories?.FirstOrDefault(x => x.Id == categoryId);

            return result;
        }

        public async Task CreateCategoryAsync(CategoryDTO itemToAdd)
        {
            var categories = await _cacheService.GetAsync<List<CategoryDTO>>(CacheConstant.AllCategory);
            if (categories == null)
                throw new NullReferenceException();

            itemToAdd.Id = categories.Count;
            categories.Add(itemToAdd);

            await _cacheService.SetAsync(CacheConstant.AllCategory, categories);
        }

        public async Task UpdateCategoryAsync(CategoryDTO itemToUpdate)
        {
            var categories = await _cacheService.GetAsync<List<CategoryDTO>>(CacheConstant.AllCategory);
            if (categories == null)
                throw new NullReferenceException();

            var index = categories.FindIndex(x => x.Id == itemToUpdate.Id);

            if (index != -1)
            {
                categories[index] = itemToUpdate;
                await _cacheService.SetAsync(CacheConstant.AllCategory, categories);
            }
        }

        public async Task DeleteCategoryAsync(CategoryDTO itemToDelete)
        {
            var categories = await _cacheService.GetAsync<List<CategoryDTO>>(CacheConstant.AllCategory);
            if (categories == null)
                throw new NullReferenceException();

            var index = categories.FindIndex(x => x.Id == itemToDelete.Id);

            if (index != -1)
            {
                categories.Remove(categories[index]);

                await _cacheService.SetAsync(CacheConstant.AllCategory, categories);
            }
        }
    }
}
