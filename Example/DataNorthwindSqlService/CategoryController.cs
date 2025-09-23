using Dapper;
using DataNorthwindSqlProvider.DTOs;
using DataNorthwindSqlService.Constants;
using DataProvider.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace DataNorthwindSqlService
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpPost]
        public async Task<List<CategoryDTO>> GetCategoriesAsync()
        {
            List<CategoryDTO> loRtn = Enumerable.Empty<CategoryDTO>().ToList();

            try
            {
                using var conn = new SqlConnection(SqlServerConstant.SqlConnectionString);
                conn.Open();

                var query = "SELECT * FROM Categories (NOLOCK)";

                var result = await conn.QueryAsync<CategoryDTO>(query);
                loRtn = result.ToList();
            }
            catch (Exception)
            {
                throw;
            }

            return loRtn;
        }

        [HttpPost]
        public async Task<CategoryDTO?> GetCategoryAsync(GetCategoryParameterDTO categoryParameter)
        {
            CategoryDTO? loRtn = null;

            try
            {
                using var conn = new SqlConnection(SqlServerConstant.SqlConnectionString);
                conn.Open();

                var query = "SELECT * FROM Categories (NOLOCK) WHERE CategoryID = @CategoryID";

                loRtn = await conn.QueryFirstOrDefaultAsync<CategoryDTO>(query, new { CategoryID = categoryParameter.CategoryID });
            }
            catch (Exception)
            {
                throw;
            }

            return loRtn;
        }

        [HttpPost]
        public Task CreateCategoryAsync(CategoryDTO itemToAdd)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public Task DeleteCategoryAsync(CategoryDTO itemToDelete)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public Task UpdateCategoryAsync(CategoryDTO itemToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
