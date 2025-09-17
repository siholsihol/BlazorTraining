using DataProvider.DTOs;
using DataProvider.Services;

namespace DataNorthwindSqlServerProvider
{
    public class CategoryService : ICategoryService
    {
        public async Task CreateCategoryAsync(CategoryDTO itemToAdd)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCategoryAsync(CategoryDTO itemToUpdate)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCategoryAsync(CategoryDTO itemToDelete)
        {
            throw new NotImplementedException();
        }

        public Task<List<CategoryDTO>> GetCategoriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CategoryDTO> GetCategoryAsync(int categoryId)
        {
            throw new NotImplementedException();
        }
    }
}
