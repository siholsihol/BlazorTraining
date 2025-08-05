using BlazorMenuCommon.DTOs;
using R_BlazorFrontEnd.Exceptions;

namespace BlazorMenu.Services
{
    public class R_MenuService : R_IMenuService
    {
        public Dictionary<string, string[]> MenuAccess { get; private set; }

        public string[] MenuIdList { get; private set; }

        private void CreateGroup(List<MenuListDTO> poExistingList, string pcMenuId, string pcMenuName, string pcSubMenuId, string pcSubMenuName)
        {
            poExistingList.Add(new MenuListDTO
            {
                CMENU_ID = pcMenuId,
                CMENU_NAME = pcMenuName,
                CPARENT_SUB_MENU_ID = pcMenuId,
                CSUB_MENU_ACCESS = "",
                CSUB_MENU_ID = pcSubMenuId,
                CSUB_MENU_NAME = pcSubMenuName,
                CSUB_MENU_TYPE = "G",
                IFAVORITE_INDEX = 0,
                IGROUP_INDEX = 0
            });
        }

        private void CreateProgram(List<MenuListDTO> poExistingList, string pcMenuId, string pcParentMenuId, string pcSubMenuId, string pcSubMenuName, string pcIconId = "")
        {
            poExistingList.Add(new MenuListDTO
            {
                CMENU_ID = pcMenuId,
                CMENU_NAME = "",
                CPARENT_SUB_MENU_ID = pcParentMenuId,
                CSUB_MENU_ACCESS = "A,U,D,P,V",
                CSUB_MENU_ID = pcSubMenuId,
                CSUB_MENU_NAME = pcSubMenuName,
                CSUB_MENU_TYPE = "P",
                IFAVORITE_INDEX = 0,
                IGROUP_INDEX = 0,
                CPROGRAM_BUTTON = pcIconId
            });
        }

        private void CreateExamplePrograms(List<MenuListDTO> poExistingList)
        {
            CreateGroup(poExistingList, "Ex", "Example", "G001", "Conductor Example");
            CreateGroup(poExistingList, "Ex", "Batch", "Batch", "Batch");
            CreateGroup(poExistingList, "Ex", "Inquiry", "Inquiry", "Inquiry");
            CreateGroup(poExistingList, "Ex", "Master", "Master", "Master");
            CreateGroup(poExistingList, "Ex", "Report", "Report", "Report");
            CreateGroup(poExistingList, "Ex", "Transaction", "Transaction", "Transaction");

            CreateProgram(poExistingList, "Ex", "G001", "SAB01000", "Gridview Batch", "default-Batch");
            CreateProgram(poExistingList, "Ex", "G001", "SAB02400", "Batch Process", "default-Inquiry");
            CreateProgram(poExistingList, "Ex", "G001", "SAB01200", "Grid and Grid", "default-Master");
            CreateProgram(poExistingList, "Ex", "G001", "SAB01400", "Detail", "default-Report");
            CreateProgram(poExistingList, "Ex", "G001", "SAB01900", "Tab Header Detail", "default-Transaction");

            CreateProgram(poExistingList, "Ex", "Batch", "SAB00800", "TreeView Navigator");
            CreateProgram(poExistingList, "Ex", "Batch", "SAB00600", "Gridview Original", "default");
            
            CreateProgram(poExistingList, "Ex", "Inquiry", "SAB00700", "Gridview Navigator");
            CreateProgram(poExistingList, "Ex", "Inquiry", "SAB00900", "Find with Conductor", "default");

            CreateProgram(poExistingList, "Ex", "Master", "SAB01300", "Navigator and Batch");
            CreateProgram(poExistingList, "Ex", "Master", "APM00100", "System Parameter", "APM00100");
            CreateProgram(poExistingList, "Ex", "Master", "APM00200", "Expenditure Master", "APM00200");
            CreateProgram(poExistingList, "Ex", "Master", "APM00300", "Supplier Master", "APM00300");
            CreateProgram(poExistingList, "Ex", "Master", "APM00400", "Supplier Master", "APM00400");
            CreateProgram(poExistingList, "Ex", "Master", "APM00500", "Supplier Master", "APM00500");
            CreateProgram(poExistingList, "Ex", "Master", "CBM00100", "System Parameter", "CBM00100");

            CreateProgram(poExistingList, "Ex", "Report", "SAB01300", "Navigator and Batch");
            CreateProgram(poExistingList, "Ex", "Report", "APR00100", "Activity", "APR00100");
            CreateProgram(poExistingList, "Ex", "Report", "APR00200", "Ageing", "APR00200");
            CreateProgram(poExistingList, "Ex", "Report", "APR00300", "Supplier Statement", "APR00300");
            CreateProgram(poExistingList, "Ex", "Report", "APR00400", "Transaction vs Tax List.", "APR00400");
            CreateProgram(poExistingList, "Ex", "Report", "APR00500", "Invoice List", "APR00500");

            CreateProgram(poExistingList, "Ex", "Transaction", "SAB01300", "Navigator and Batch");
            CreateProgram(poExistingList, "Ex", "Transaction", "APT00100", "Purchase Invoice", "APT00100");
            CreateProgram(poExistingList, "Ex", "Transaction", "APT00200", "Purchase Return", "APT00200");
            CreateProgram(poExistingList, "Ex", "Transaction", "APT00300", "Purchase Debit Note", "APT00300");
            CreateProgram(poExistingList, "Ex", "Transaction", "APT00400", "Purchase Credit Note", "APT00400");
            CreateProgram(poExistingList, "Ex", "Transaction", "APT00500", "Purchase Debit Adjustment", "APT00500");
            CreateProgram(poExistingList, "Ex", "Transaction", "APT00600", "Purchase Credit Adjustment", "APT00600");
            CreateProgram(poExistingList, "Ex", "Transaction", "APT00800", "Tax In Record", "APT00800");
            CreateProgram(poExistingList, "Ex", "Transaction", "APT00900", "Import Purchase Invoice", "APT00900");
            CreateProgram(poExistingList, "Ex", "Transaction", "APT01000", "Scheduling Payment", "APT01000");
            CreateProgram(poExistingList, "Ex", "Transaction", "APT01100", "Cash Payment to Supplier", "APT01100");
            CreateProgram(poExistingList, "Ex", "Transaction", "APT01200", "Wire Transfer Payment to Supplier", "APT01200");
            CreateProgram(poExistingList, "Ex", "Transaction", "APT01300", "Cheque Payment to Supplier", "APT01300");
            CreateProgram(poExistingList, "Ex", "Transaction", "CBT00100", "Cash Receipt Journal", "CBT00100");
            CreateProgram(poExistingList, "Ex", "Transaction", "CBT00200", "Cash Payment Journal", "CBT00200");
            CreateProgram(poExistingList, "Ex", "Transaction", "CBT01100", "Receipt Journal", "CBT01100");
            CreateProgram(poExistingList, "Ex", "Transaction", "CBT01200", "Wire Payment Journal", "CBT01200");
            CreateProgram(poExistingList, "Ex", "Transaction", "CBT02100", "Check Receipt Journal", "CBT02100");
            CreateProgram(poExistingList, "Ex", "Transaction", "CBT02200", "Check Payment Journal", "CBT02200");
            CreateProgram(poExistingList, "Ex", "Transaction", "CBT02300", "Bank In Check", "CBT02300");
            CreateProgram(poExistingList, "Ex", "Transaction", "CBT03100", "Cash Bank Transfer Internal", "CBT03100");
            CreateProgram(poExistingList, "Ex", "Transaction", "FAM00300", "Void CB Transaction", "FAM00300");
        }

        private void CreateControlPrograms(List<MenuListDTO> poExistingList)
        {
            CreateGroup(poExistingList, "CT", "Controls", "G001", "Controls");

            CreateProgram(poExistingList, "CT", "G001", "SAB03000", "Tab");
            CreateProgram(poExistingList, "CT", "G001", "SAB03100", "Input Controls");
            CreateProgram(poExistingList, "CT", "G001", "SAB03200", "Buttons");

            CreateProgram(poExistingList, "CT", "G001", "SAB03300", "Grid Sequence");
            CreateProgram(poExistingList, "CT", "G001", "SAB03400", "Grid Mover");
        }

        private void CreateOtherPrograms(List<MenuListDTO> poExistingList)
        {
            CreateGroup(poExistingList, "OT", "Others", "G001", "Others");

            CreateProgram(poExistingList, "OT", "G001", "SAB04000", "Excel");
            CreateProgram(poExistingList, "OT", "G001", "SAB04000", "Excel");
            CreateProgram(poExistingList, "OT", "G001", "SAB04000", "Excel");
            CreateProgram(poExistingList, "OT", "G001", "SAB04000", "Excel");
            CreateProgram(poExistingList, "OT", "G001", "SAB04000", "Excel");
            CreateProgram(poExistingList, "OT", "G001", "SAB04000", "Excel");
            CreateProgram(poExistingList, "OT", "G001", "SAB04000", "Excel");
            CreateProgram(poExistingList, "OT", "G001", "SAB04000", "Excel");
        }

        public Task<List<MenuListDTO>> GetMenuAsync()
        {
            var loEx = new R_Exception();
            List<MenuListDTO> loResult = null;

            try
            {
                loResult = new List<MenuListDTO>();

                CreateExamplePrograms(loResult);

                CreateControlPrograms(loResult);

                CreateOtherPrograms(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return Task.FromResult(loResult);
        }
    }
}
