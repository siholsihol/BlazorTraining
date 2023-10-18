using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;
using SAB00800Front.DTOs;
using System.Collections.ObjectModel;

namespace SAB00800Front
{
    public class SAB00800ViewModel : R_ViewModel<TreeDetailDTO>
    {
        public ObservableCollection<TreeDTO> TenantList = new();
        public TreeDetailDTO Tenant = new();

        public void GetTenantList()
        {
            var loEx = new R_Exception();

            try
            {
                TenantList = new ObservableCollection<TreeDTO>(GetFlatData());
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
                var currentTenant = TenantList.FirstOrDefault(x => x.CCATEGORY_ID == pcCategoryId);
                Tenant = R_FrontUtility.ConvertObjectToObject<TreeDetailDTO>(currentTenant);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public void SaveCategory(TreeDetailDTO poEntity, eCRUDMode peCRUDMode)
        {
            var loEx = new R_Exception();

            try
            {
                if (peCRUDMode == eCRUDMode.AddMode)
                {
                    var loSaveTenant = R_FrontUtility.ConvertObjectToObject<TreeDTO>(poEntity);
                    TenantList.Add(loSaveTenant);
                }
                else
                {
                    //CategoryService.UpdateCategory(poEntity);
                }

                Tenant = poEntity;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private List<TreeDTO> GetFlatData()
        {
            List<TreeDTO> items = new List<TreeDTO>();

            items.Add(new TreeDTO()
            {
                CPARENT = null,
                CCATEGORY_ID = "C2001",
                CCATEGORY_NAME = "Metro Park",
                ILEVEL = 0
            });
            items.Add(new TreeDTO()
            {
                CPARENT = "C2001",
                CCATEGORY_ID = "C2011",
                CCATEGORY_NAME = "Tower 1",
                ILEVEL = 1
            });
            items.Add(new TreeDTO()
            {
                CPARENT = "C2001",
                CCATEGORY_ID = "C2012",
                CCATEGORY_NAME = "Tower 2",
                ILEVEL = 1
            });
            items.Add(new TreeDTO()
            {
                CPARENT = "C2001",
                CCATEGORY_ID = "C2013",
                CCATEGORY_NAME = "Tower 3",
                ILEVEL = 1
            });
            items.Add(new TreeDTO()
            {
                CPARENT = "C2001",
                CCATEGORY_ID = "CTG01",
                CCATEGORY_NAME = "Tenant",
                ILEVEL = 1
            });
            items.Add(new TreeDTO()
            {
                CPARENT = null,
                CCATEGORY_ID = "C2002",
                CCATEGORY_NAME = "Parent 2",
                ILEVEL = 0
            });

            items.Where(x => string.IsNullOrWhiteSpace(x.CPARENT) && items.Where(y => y.CPARENT == x.CCATEGORY_ID).Count() > 0).ToList().ForEach(x => x.LHAS_CHILDREN = true);

            items.ForEach(x => x.CCATEGORY_NAME = $"[{x.ILEVEL}] {x.CCATEGORY_ID} - {x.CCATEGORY_NAME}");

            return items;
        }
    }
}
