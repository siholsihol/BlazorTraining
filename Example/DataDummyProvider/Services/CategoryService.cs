using Bogus;
using DataProvider.DTOs;
using DataProvider.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataDummyProvider.Services
{
    public class CategoryService : ICategoryService
    {
        private static List<CategoryDTO> _categories = new List<CategoryDTO>();

        public Task<List<CategoryDTO>> GetCategoriesAsync()
        {
            if (_categories.Count != 0)
                return Task.FromResult(_categories);

            var startId = 1;

            var faker = new Faker<CategoryDTO>()
                    .RuleFor(u => u.Id, f => startId++)
                    .RuleFor(u => u.Name, f => f.Commerce.Department())
                    .RuleFor(u => u.Description, f => f.Commerce.ProductDescription());

            _categories = faker.Generate(3);

            return Task.FromResult(_categories);
        }

        public Task<CategoryDTO> GetCategoryAsync(int categoryId)
        {
            var result = _categories.FirstOrDefault(x => x.Id == categoryId);

            return Task.FromResult(result);
        }

        public Task CreateCategoryAsync(CategoryDTO itemToAdd)
        {
            _categories.Add(itemToAdd);

            return Task.CompletedTask;
        }

        public Task UpdateCategoryAsync(CategoryDTO itemToUpdate)
        {
            var index = _categories.FindIndex(x => x.Id == itemToUpdate.Id);

            if (index != -1)
                _categories[index] = itemToUpdate;

            return Task.CompletedTask;
        }

        public Task DeleteCategoryAsync(CategoryDTO itemToDelete)
        {
            var index = _categories.FindIndex(x => x.Id == itemToDelete.Id);

            if (index != -1)
                _categories.Remove(_categories[index]);

            return Task.CompletedTask;
        }
    }
}
