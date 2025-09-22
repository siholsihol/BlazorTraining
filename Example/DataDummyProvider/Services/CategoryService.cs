using Bogus;
using DataProvider.Cache;
using DataProvider.Constants;
using DataProvider.DTOs;
using DataProvider.Extensions;
using DataProvider.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataDummyProvider.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICacheService _cacheService;

        public CategoryService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<List<CategoryDTO>> GetCategoriesAsync()
        {
            var categories = await _cacheService.GetOrSetAsync(
                CacheConstant.AllCategory,
                GetCategories);

            return categories;
        }

        private Task<List<CategoryDTO>> GetCategories()
        {
            var startId = 1;

            var faker = new Faker<CategoryDTO>()
                    .RuleFor(u => u.Id, f => startId++)
                    .RuleFor(u => u.Name, f => f.Commerce.Categories(1)[0])
                    .RuleFor(u => u.Description, f => f.Lorem.Sentence(10));

            var result = faker.Generate(3);

            return Task.FromResult(result);
        }

        public async Task<CategoryDTO> GetCategoryAsync(int categoryId)
        {
            var categories = await _cacheService.GetAsync<List<CategoryDTO>>(CacheConstant.AllCategory);
            var result = categories.FirstOrDefault(x => x.Id == categoryId);

            return result;
        }

        public async Task CreateCategoryAsync(CategoryDTO itemToAdd)
        {
            var categories = await _cacheService.GetAsync<List<CategoryDTO>>(CacheConstant.AllCategory);
            itemToAdd.Id = categories.Count;
            categories.Add(itemToAdd);

            await _cacheService.SetAsync(CacheConstant.AllCategory, categories);
        }

        public async Task UpdateCategoryAsync(CategoryDTO itemToUpdate)
        {
            var categories = await _cacheService.GetAsync<List<CategoryDTO>>(CacheConstant.AllCategory);
            var index = categories.FindIndex(x => x.Id == itemToUpdate.Id);

            if (index != -1)
                categories[index] = itemToUpdate;

            await _cacheService.SetAsync(CacheConstant.AllCategory, categories);
        }

        public async Task DeleteCategoryAsync(CategoryDTO itemToDelete)
        {
            var categories = await _cacheService.GetAsync<List<CategoryDTO>>(CacheConstant.AllCategory);
            var index = categories.FindIndex(x => x.Id == itemToDelete.Id);

            if (index != -1)
                categories.Remove(categories[index]);

            await _cacheService.SetAsync(CacheConstant.AllCategory, categories);
        }
    }
}
