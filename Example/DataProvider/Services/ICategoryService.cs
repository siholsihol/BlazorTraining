using DataProvider.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataProvider.Services
{
    public interface ICategoryService
    {
        public Task CreateCategoryAsync(CategoryDTO itemToAdd);
        public Task UpdateCategoryAsync(CategoryDTO itemToUpdate);
        public Task DeleteCategoryAsync(CategoryDTO itemToDelete);
        public Task<List<CategoryDTO>> GetCategoriesAsync();
        public Task<CategoryDTO> GetCategoryAsync(int categoryId);
    }
}