using DataProvider.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataProvider.Services
{
    public interface ISupplierService
    {
        public Task<List<SupplierDTO>> GetSuppliersAsync();
    }
}