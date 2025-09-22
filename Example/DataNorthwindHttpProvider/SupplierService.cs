using DataProvider.Cache;
using DataProvider.Constants;
using DataProvider.DTOs;
using DataProvider.Extensions;
using DataProvider.Services;
using System.Net.Http.Json;

namespace DataNorthwindHttpProvider
{
    public class SupplierService : ISupplierService
    {
        private const string _jsonFile = "sample-data/suppliers.json";

        private readonly ICacheService _cacheService;
        private readonly HttpClient _httpClient;

        public SupplierService(
            ICacheService cacheService,
            HttpClient httpClient)
        {
            _cacheService = cacheService;
            _httpClient = httpClient;
        }

        public async Task<List<SupplierDTO>> GetSuppliersAsync()
        {
            var suppliers = await _cacheService.GetOrSetAsync(
                CacheConstant.AllSupplier,
                GetSuppliers);

            return suppliers ?? Enumerable.Empty<SupplierDTO>().ToList();
        }

        private async Task<List<SupplierDTO>> GetSuppliers()
        {
            var result = await _httpClient.GetFromJsonAsync<List<SupplierDTO>>(_jsonFile);

            return result ?? Enumerable.Empty<SupplierDTO>().ToList();
        }
    }
}
