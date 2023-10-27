using DataDummyProvider.DTOs;
using DataDummyProvider.Services;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using SAB00800Front.DTOs;

namespace SAB00800Front
{
    public class SAB00800ViewModel : R_ViewModel<TenantDTO>
    {
        public List<TenantTreeDTO> TenantList = new();
        public TenantDTO Tenant = new();
        public IEnumerable<object> ExpandTenantList = Enumerable.Empty<object>();

        public void GetTenantList()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = TenantService.GetTenants();

                var loGridData = loResult.Select(x =>
                new TenantTreeDTO
                {
                    CPARENT = x.CPARENT,
                    CCATEGORY_ID = x.CCATEGORY_ID,
                    CCATEGORY_NAME_DISPLAY = $"[{x.ILEVEL}] {x.CCATEGORY_ID} - {x.CCATEGORY_NAME}",
                    LHAS_CHILDREN = string.IsNullOrWhiteSpace(x.CPARENT) && loResult.Where(y => y.CPARENT == x.CCATEGORY_ID).Count() > 0
                });

                TenantList = loGridData.ToList();

                ExpandTenantList = loGridData.Where(x => x.LHAS_CHILDREN == true).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public void GetTenantById(string pcCategoryId)
        {
            var loEx = new R_Exception();

            try
            {
                Tenant = TenantService.GetTenant(pcCategoryId);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public void SaveTenant(TenantDTO poEntity, eCRUDMode peCRUDMode)
        {
            var loEx = new R_Exception();

            try
            {
                if (peCRUDMode == eCRUDMode.AddMode)
                {
                    TenantService.CreateTenant(poEntity);
                }
                else
                {
                    TenantService.UpdateTenant(poEntity);
                }

                Tenant = poEntity;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public void DeleteTenant(string pcCategoryId)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = new TenantDTO { CCATEGORY_ID = pcCategoryId };
                TenantService.DeleteTenant(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}
