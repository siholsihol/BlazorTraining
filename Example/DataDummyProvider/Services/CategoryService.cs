using Bogus;
using DataDummyProvider.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace DataDummyProvider.Services
{
    public static class CategoryService
    {
        private static List<CategoryDTO> _categories = new List<CategoryDTO>();

        public static List<CategoryDTO> GetCategories()
        {
            if (_categories.Count != 0)
                return _categories;

            var startId = 1;

            var faker = new Faker<CategoryDTO>()
                    .RuleFor(u => u.Id, f => startId++)
                    .RuleFor(u => u.Name, f => f.Commerce.Department())
                    .RuleFor(u => u.Description, f => f.Commerce.ProductDescription());

            _categories = faker.Generate(3);

            return _categories;
        }

        public static CategoryDTO GetCategory(int categoryId)
        {
            return _categories.FirstOrDefault(x => x.Id == categoryId);
        }

        public static void CreateCategory(CategoryDTO itemToAdd)
        {
            _categories.Add(itemToAdd);
        }

        public static void UpdateCategory(CategoryDTO itemToUpdate)
        {
            var index = _categories.FindIndex(x => x.Id == itemToUpdate.Id);

            if (index != -1)
                _categories[index] = itemToUpdate;
        }

        public static void DeleteCategory(CategoryDTO itemToDelete)
        {
            var index = _categories.FindIndex(x => x.Id == itemToDelete.Id);

            if (index != -1)
                _categories.Remove(_categories[index]);
        }
    }
}
