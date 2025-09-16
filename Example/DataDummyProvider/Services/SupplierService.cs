using Bogus;
using DataProvider.DTOs;
using DataProvider.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataDummyProvider.Services
{
    public class SupplierService : ISupplierService
    {
        private static List<SupplierDTO> _suppliers = new List<SupplierDTO>();
        private readonly IProductService _productService;

        public SupplierService(IProductService productService)
        {
            _productService = productService;
        }

        public Task<List<SupplierDTO>> GetSuppliersAsync()
        {
            if (_suppliers.Count != 0)
                return Task.FromResult(_suppliers);

            var faker = new Faker<SupplierDTO>()
            .RuleFor(u => u.Id, f => f.IndexFaker)
            .RuleFor(u => u.CompanyName, f => f.Company.CompanyName());

            _suppliers = faker.Generate(10);

            return Task.FromResult(_suppliers);
        }

        public async Task<List<SupplierDTO>> GetSuppliersByCategoryAsync(int categoryId)
        {
            List<SupplierDTO> loResult = _suppliers;

            if (_suppliers.Count == 0)
                loResult = await GetSuppliersAsync();

            var loProducts = await _productService.GetProductsByCategoryAsync(categoryId);
            var loSupplierByProduct = loProducts.GroupBy(x => x.SupplierId).Select(x => x.First().SupplierId);

            var query = from supplier in loResult
                        join supplierByProduct in loSupplierByProduct
                        on supplier.Id equals supplierByProduct
                        select new SupplierDTO { Id = supplier.Id, CompanyName = supplier.CompanyName };

            loResult = query.ToList();

            return loResult;
        }
    }
}
