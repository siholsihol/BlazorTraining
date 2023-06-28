using Bogus;
using DataDummyProvider.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace DataDummyProvider.Services
{
    public class SupplierService
    {
        private static List<SupplierDTO> _suppliers = new List<SupplierDTO>();

        public static List<SupplierDTO> GetSuppliers()
        {
            if (_suppliers.Count != 0)
                return _suppliers;

            var faker = new Faker<SupplierDTO>()
            .RuleFor(u => u.Id, f => f.IndexFaker)
            .RuleFor(u => u.CompanyName, f => f.Company.CompanyName());

            _suppliers = faker.Generate(10);

            return _suppliers;
        }

        public static List<SupplierDTO> GetSuppliersByCategory(int categoryId)
        {
            List<SupplierDTO> loResult = _suppliers;

            if (_suppliers.Count == 0)
                loResult = GetSuppliers();

            var loProducts = ProductService.GetProductsByCategory(categoryId);
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
