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
                CSUB_MENU_TOOL_TIP = $"{pcSubMenuId} - {pcSubMenuName}",
                CSUB_MENU_TYPE = "G",
                IFAVORITE_INDEX = 0,
                IGROUP_INDEX = 0
            });
        }

        private void CreateProgram(List<MenuListDTO> poExistingList, string pcMenuId, string pcParentMenuId, string pcSubMenuId, string pcSubMenuName)
        {
            poExistingList.Add(new MenuListDTO
            {
                CMENU_ID = pcMenuId,
                CMENU_NAME = "",
                CPARENT_SUB_MENU_ID = pcParentMenuId,
                CSUB_MENU_ACCESS = "A,U,D,P,V",
                CSUB_MENU_ID = pcSubMenuId,
                CSUB_MENU_NAME = pcSubMenuName,
                CSUB_MENU_TOOL_TIP = $"{pcSubMenuId} - {pcSubMenuName}",
                CSUB_MENU_TYPE = "P",
                IFAVORITE_INDEX = 0,
                IGROUP_INDEX = 0
            });
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
        }

        public Task<List<MenuListDTO>> GetMenuAsync()
        {
            var loEx = new R_Exception();
            List<MenuListDTO> loResult = null;

            try
            {
                loResult = new List<MenuListDTO>();

                void AddMenu(string id, string name, string type = "P")
                {
                    loResult.Add(new MenuListDTO
                    {
                        CMENU_ID = "Ex",
                        CMENU_NAME = "Example",
                        CPARENT_SUB_MENU_ID = "G001",
                        CSUB_MENU_ACCESS = "A,U,D,P,V",
                        CSUB_MENU_ID = id,
                        CSUB_MENU_NAME = name,
                        CSUB_MENU_TOOL_TIP = $"{id} - {name}",
                        CSUB_MENU_TYPE = type,
                        IFAVORITE_INDEX = 0,
                        IGROUP_INDEX = 0
                    });
                }

                AddMenu("G001", "Conductor Example", "G");
                AddMenu("SAB00100", "Layout Training");
                AddMenu("SAB00900", "Find with Conductor");
                AddMenu("SAB00700", "Gridview Navigator");
                AddMenu("SAB00600", "Gridview Original");
                AddMenu("SAB01000", "Gridview Batch");
                AddMenu("SAB02400", "Batch Process");
                AddMenu("SAB01200", "Grid and Grid");
                AddMenu("SAB01400", "Detail");
                AddMenu("SAB01900", "Tab Header Detail");
                AddMenu("SAB00800", "TreeView Navigator");
                AddMenu("SAB01300", "Navigator and Batch");
                AddMenu("SAB01500", "Navigator and Navigator");

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
