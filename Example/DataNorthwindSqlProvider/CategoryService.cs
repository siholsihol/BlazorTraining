using DataNorthwindSqlProvider.Constants;
using DataNorthwindSqlProvider.DTOs;
using DataProvider.DTOs;
using DataProvider.Services;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataNorthwindSqlProvider
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(HttpClientConstant.Name);
        }

        public async Task<List<CategoryDTO>> GetCategoriesAsync()
        {
            List<CategoryDTO>? result = null;

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, HttpRouteCategory.GetCategories);
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<List<CategoryDTO>>(content);
            }
            catch (Exception)
            {
                throw;
            }

            return result ?? Enumerable.Empty<CategoryDTO>().ToList();
        }

        public async Task<CategoryDTO?> GetCategoryAsync(int categoryId)
        {
            CategoryDTO? result = null;

            try
            {
                var parameter = new GetCategoryParameterDTO { CategoryID = categoryId };

                var response = await _httpClient.PostAsJsonAsync(HttpRouteCategory.GetCategory, parameter);
                var responseAsString = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<CategoryDTO>(responseAsString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public Task CreateCategoryAsync(CategoryDTO itemToAdd)
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
    }
}
