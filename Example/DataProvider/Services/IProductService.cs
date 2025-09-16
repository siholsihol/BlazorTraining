using DataProvider.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataProvider.Services
{
    public interface IProductService
    {
        public Task CreateProductAsync(ProductDTO itemToAdd);
        public Task UpdateProductAsync(ProductDTO itemToUpdate);
        public Task DeleteProductAsync(ProductDTO itemToDelete);
        public Task<List<ProductDTO>> GetNewProductsAsync();
        public Task<ProductDTO> GetProductAsync(int productId);
        public Task<List<ProductDTO>> GetProductsAsync();
        public Task<List<ProductDTO>> GetProductsByCategoryAsync(int categoryId);
    }
}