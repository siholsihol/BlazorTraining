using Dapper;
using DataNorthwindSqliteProvider.Helper;
using DataProvider.DTOs;
using DataProvider.Services;

namespace DataNorthwindSqliteProvider
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _dataContext;

        public CategoryService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task CreateCategoryAsync(CategoryDTO itemToAdd)
        {
            try
            {
                using var connection = _dataContext.CreateConnection();

                var sql = "INSERT INTO Categories (CategoryName, Description) VALUES (@CategoryName, @Description)";
                var affectedRows = await connection.ExecuteAsync(sql,
                    new
                    {
                        CategoryName = itemToAdd.Name,
                        Description = itemToAdd.Description
                    });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateCategoryAsync(CategoryDTO itemToUpdate)
        {
            try
            {
                using var connection = _dataContext.CreateConnection();

                var sql = "UPDATE Categories SET CategoryName = @CategoryName, Description = @Description WHERE CategoryID = @CategoryID";
                var affectedRows = await connection.ExecuteAsync(sql,
                    new
                    {
                        CategoryName = itemToUpdate.Name,
                        Description = itemToUpdate.Description,
                        CategoryID = itemToUpdate.Id
                    });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteCategoryAsync(CategoryDTO itemToDelete)
        {
            try
            {
                using var connection = _dataContext.CreateConnection();

                var sql = "DELETE FROM Categories WHERE CategoryID = @CategoryID";
                var affectedRows = await connection.ExecuteAsync(sql,
                    new
                    {
                        CategoryID = itemToDelete.Id
                    });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CategoryDTO>> GetCategoriesAsync()
        {
            try
            {
                using var connection = _dataContext.CreateConnection();

                var sql = "SELECT * FROM Categories";
                var customers = await connection.QueryAsync<CategoryDTO>(sql);

                return customers.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CategoryDTO> GetCategoryAsync(int categoryId)
        {
            try
            {
                using var connection = _dataContext.CreateConnection();

                var sql = "SELECT * FROM Categories WHERE CategoryID = @CategoryID";
                var customer = await connection.QueryFirstOrDefaultAsync<CategoryDTO>(sql,
                    new
                    {
                        CategoryID = categoryId
                    });

                return customer;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
