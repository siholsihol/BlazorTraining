using DataDummyProvider.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace DataDummyProvider.Services
{
    public static class TenantService
    {
        private static List<TenantDTO> _tenants = new List<TenantDTO>();

        public static List<TenantDTO> GetTenants()
        {
            if (_tenants.Count != 0)
                return _tenants;

            _tenants = GetFlatData();

            return _tenants;
        }

        public static TenantDTO GetTenant(string tenantId)
        {
            return _tenants.FirstOrDefault(x => x.CCATEGORY_ID == tenantId);
        }

        public static void CreateTenant(TenantDTO itemToAdd)
        {
            _tenants.Add(itemToAdd);
        }

        public static void UpdateTenant(TenantDTO itemToUpdate)
        {
            var index = _tenants.FindIndex(x => x.CCATEGORY_ID == itemToUpdate.CCATEGORY_ID);

            if (index != -1)
                _tenants[index] = itemToUpdate;
        }

        public static void DeleteTenant(TenantDTO itemToDelete)
        {
            var index = _tenants.FindIndex(x => x.CCATEGORY_ID == itemToDelete.CCATEGORY_ID);

            if (index != -1)
                _tenants.Remove(_tenants[index]);
        }

        private static List<TenantDTO> GetFlatData()
        {
            List<TenantDTO> items = new List<TenantDTO>();

            items.Add(new TenantDTO()
            {
                CPARENT = null,
                CCATEGORY_ID = "C2001",
                CCATEGORY_NAME = "Metro Park",
                ILEVEL = 0
            });
            items.Add(new TenantDTO()
            {
                CPARENT = "C2001",
                CCATEGORY_ID = "C2011",
                CCATEGORY_NAME = "Tower 1",
                ILEVEL = 1
            });
            items.Add(new TenantDTO()
            {
                CPARENT = "C2001",
                CCATEGORY_ID = "C2012",
                CCATEGORY_NAME = "Tower 2",
                ILEVEL = 1
            });
            items.Add(new TenantDTO()
            {
                CPARENT = "C2001",
                CCATEGORY_ID = "C2013",
                CCATEGORY_NAME = "Tower 3",
                ILEVEL = 1
            });
            items.Add(new TenantDTO()
            {
                CPARENT = "C2001",
                CCATEGORY_ID = "CTG01",
                CCATEGORY_NAME = "Tenant",
                ILEVEL = 1
            });
            items.Add(new TenantDTO()
            {
                CPARENT = null,
                CCATEGORY_ID = "C2002",
                CCATEGORY_NAME = "Parent 2",
                ILEVEL = 0
            });

            //items.Where(x => string.IsNullOrWhiteSpace(x.CPARENT) && items.Where(y => y.CPARENT == x.CCATEGORY_ID).Count() > 0).ToList().ForEach(x => x.LHAS_CHILDREN = true);

            //items.ForEach(x => x.CCATEGORY_NAME = $"[{x.ILEVEL}] {x.CCATEGORY_ID} - {x.CCATEGORY_NAME}");

            items.ForEach(x => x.CNOTE = $"Notes {x.CCATEGORY_ID}");

            return items;
        }
    }
}
