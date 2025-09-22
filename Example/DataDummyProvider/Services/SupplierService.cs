using Bogus;
using DataProvider.Cache;
using DataProvider.Constants;
using DataProvider.DTOs;
using DataProvider.Extensions;
using DataProvider.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataDummyProvider.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ICacheService _cacheService;

        public SupplierService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<List<SupplierDTO>> GetSuppliersAsync()
        {
            var suppliers = await _cacheService.GetOrSetAsync(
                CacheConstant.AllSupplier,
                GetSuppliers);

            return suppliers ?? Enumerable.Empty<SupplierDTO>().ToList();
        }

        private Task<List<SupplierDTO>> GetSuppliers()
        {
            var faker = new Faker<SupplierDTO>()
            .RuleFor(u => u.Id, f => f.IndexFaker)
            .RuleFor(u => u.CompanyName, f => f.Company.CompanyName());

            var suppliers = faker.Generate(10);

            return Task.FromResult(suppliers);
        }
    }
}
