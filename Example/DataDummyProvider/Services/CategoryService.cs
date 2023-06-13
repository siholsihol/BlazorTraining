using Bogus;
using DataDummyProvider.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace DataDummyProvider.Services
{
    public static class CategoryService
    {
        private static List<CategoryDTO> _categories = new List<CategoryDTO>();

        public static List<CategoryDTO> GenerateCategory()
        {
            if (_categories.Count != 0)
                return GetCategories();

            for (int i = 1; i <= 3; i++)
            {
                var faker = new Faker<CategoryDTO>()
                    .RuleFor(u => u.Id, f => i)
                    .RuleFor(u => u.Name, f => f.Commerce.Categories(1).FirstOrDefault())
                    .RuleFor(u => u.Description, f => f.Commerce.ProductDescription());

                _categories.Add(faker.Generate(1).FirstOrDefault());
            };

            return GetCategories();
        }

        public static List<CategoryDTO> GetCategories()
        {
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
