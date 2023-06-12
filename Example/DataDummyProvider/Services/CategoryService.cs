using DataDummyProvider.DTOs;
using System.Collections.Generic;

namespace DataDummyProvider.Services
{
    public static class CategoryService
    {
        private static List<CategoryDTO> _categories = new List<CategoryDTO>();

        public static List<CategoryDTO> GenerateCategory()
        {
            if (_categories.Count != 0)
                return GetCategories();

            var loCategories = new List<CategoryDTO>()
            {
                new CategoryDTO { Id=1, Name = "Category 1"},
                new CategoryDTO { Id=2, Name = "Category 2"},
                new CategoryDTO { Id=3, Name = "Category 3"}
            };

            _categories = loCategories;

            return GetCategories();
        }

        public static List<CategoryDTO> GetCategories()
        {
            return _categories;
        }
    }
}
