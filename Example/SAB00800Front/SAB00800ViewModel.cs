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
        //public ObservableCollection<TenantTreeDTO> TenantList = new();
        public List<TenantTreeDTO> TenantList = new();
        public TenantDTO Tenant = new();

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

                //loGridData.Where(x => string.IsNullOrWhiteSpace(x.CPARENT) && loGridData.Where(y => y.CPARENT == x.CCATEGORY_ID).Count() > 0).ToList().ForEach(x => x.LHAS_CHILDREN = true);
                //loGridData.ForEach(x => x.CCATEGORY_NAME_DISPLAY = $"[{x.ILEVEL}] {x.CCATEGORY_ID} - {x.CCATEGORY_NAME}");

                TenantList = loGridData.ToList();
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

        public void SaveCategory(TenantDTO poEntity, eCRUDMode peCRUDMode)
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
    }
}
