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

            CreateProgram(poExistingList, "Ex", "G001", "SAB00600", "Gridview Original", "fav");
            CreateProgram(poExistingList, "Ex", "G001", "SAB00700", "Gridview Navigator");
            CreateProgram(poExistingList, "Ex", "G001", "SAB00900", "Find with Conductor");

            CreateProgram(poExistingList, "Ex", "G001", "SAB01000", "Gridview Batch");
            CreateProgram(poExistingList, "Ex", "G001", "SAB02400", "Batch Process");
            CreateProgram(poExistingList, "Ex", "G001", "SAB01200", "Grid and Grid");

            CreateProgram(poExistingList, "Ex", "G001", "SAB01400", "Detail");
            CreateProgram(poExistingList, "Ex", "G001", "SAB01900", "Tab Header Detail");
            CreateProgram(poExistingList, "Ex", "G001", "SAB00800", "TreeView Navigator");

            CreateProgram(poExistingList, "Ex", "G001", "SAB01300", "Navigator and Batch");
            CreateProgram(poExistingList, "Ex", "G001", "SAB01500", "Navigator and Navigator");
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
