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

                loResult.Add(new MenuListDTO
                {
                    CMENU_ID = "Ex",
                    CMENU_NAME = "Example",
                    CPARENT_SUB_MENU_ID = "Ex",
                    CSUB_MENU_ACCESS = "A,U,D,P,V",
                    CSUB_MENU_ID = "G001",
                    CSUB_MENU_NAME = "Conductor Example",
                    CSUB_MENU_TYPE = "G",
                    IFAVORITE_INDEX = 0,
                    IGROUP_INDEX = 1
                });

                loResult.Add(new MenuListDTO
                {
                    CMENU_ID = "Ex",
                    CMENU_NAME = "Example",
                    CPARENT_SUB_MENU_ID = "G001",
                    CSUB_MENU_ACCESS = "A,U,D,P,V",
                    CSUB_MENU_ID = "SAB00600",
                    CSUB_MENU_NAME = "Gridview Original",
                    CSUB_MENU_TYPE = "P",
                    IFAVORITE_INDEX = 0,
                    IGROUP_INDEX = 0
                });

                loResult.Add(new MenuListDTO
                {
                    CMENU_ID = "Ex",
                    CMENU_NAME = "Example",
                    CPARENT_SUB_MENU_ID = "G001",
                    CSUB_MENU_ACCESS = "A,U,D,P,V",
                    CSUB_MENU_ID = "SAB00700",
                    CSUB_MENU_NAME = "Gridview Navigator",
                    CSUB_MENU_TYPE = "P",
                    IFAVORITE_INDEX = 0,
                    IGROUP_INDEX = 0
                });

                loResult.Add(new MenuListDTO
                {
                    CMENU_ID = "Ex",
                    CMENU_NAME = "Example",
                    CPARENT_SUB_MENU_ID = "G001",
                    CSUB_MENU_ACCESS = "A,U,D,P,V",
                    CSUB_MENU_ID = "SAB00900",
                    CSUB_MENU_NAME = "Find with Conductor",
                    CSUB_MENU_TYPE = "P",
                    IFAVORITE_INDEX = 0,
                    IGROUP_INDEX = 0
                });

                loResult.Add(new MenuListDTO
                {
                    CMENU_ID = "Ex",
                    CMENU_NAME = "Example",
                    CPARENT_SUB_MENU_ID = "G001",
                    CSUB_MENU_ACCESS = "A,U,D,P,V",
                    CSUB_MENU_ID = "SAB01000",
                    CSUB_MENU_NAME = "Gridview Batch",
                    CSUB_MENU_TYPE = "P",
                    IFAVORITE_INDEX = 0,
                    IGROUP_INDEX = 0
                });

                loResult.Add(new MenuListDTO
                {
                    CMENU_ID = "Ex",
                    CMENU_NAME = "Example",
                    CPARENT_SUB_MENU_ID = "G001",
                    CSUB_MENU_ACCESS = "A,U,D,P,V",
                    CSUB_MENU_ID = "SAB02400",
                    CSUB_MENU_NAME = "Batch Process",
                    CSUB_MENU_TYPE = "P",
                    IFAVORITE_INDEX = 0,
                    IGROUP_INDEX = 0
                });

                loResult.Add(new MenuListDTO
                {
                    CMENU_ID = "Ex",
                    CMENU_NAME = "Example",
                    CPARENT_SUB_MENU_ID = "G001",
                    CSUB_MENU_ACCESS = "A,U,D,P,V",
                    CSUB_MENU_ID = "SAB01200",
                    CSUB_MENU_NAME = "Grid and Grid",
                    CSUB_MENU_TYPE = "P",
                    IFAVORITE_INDEX = 0,
                    IGROUP_INDEX = 0
                });

                loResult.Add(new MenuListDTO
                {
                    CMENU_ID = "Ex",
                    CMENU_NAME = "Example",
                    CPARENT_SUB_MENU_ID = "G001",
                    CSUB_MENU_ACCESS = "A,U,D,P,V",
                    CSUB_MENU_ID = "SAB01400",
                    CSUB_MENU_NAME = "Detail",
                    CSUB_MENU_TYPE = "P",
                    IFAVORITE_INDEX = 0,
                    IGROUP_INDEX = 0
                });

                loResult.Add(new MenuListDTO
                {
                    CMENU_ID = "Ex",
                    CMENU_NAME = "Example",
                    CPARENT_SUB_MENU_ID = "G001",
                    CSUB_MENU_ACCESS = "A,U,D,P,V",
                    CSUB_MENU_ID = "SAB01900",
                    CSUB_MENU_NAME = "Tab Header Detail",
                    CSUB_MENU_TYPE = "P",
                    IFAVORITE_INDEX = 0,
                    IGROUP_INDEX = 0
                });

                loResult.Add(new MenuListDTO
                {
                    CMENU_ID = "Ex",
                    CMENU_NAME = "Example",
                    CPARENT_SUB_MENU_ID = "G001",
                    CSUB_MENU_ACCESS = "A,U,D,P,V",
                    CSUB_MENU_ID = "SAB00800",
                    CSUB_MENU_NAME = "TreeView Navigator",
                    CSUB_MENU_TYPE = "P",
                    IFAVORITE_INDEX = 0,
                    IGROUP_INDEX = 0
                });

                loResult.Add(new MenuListDTO
                {
                    CMENU_ID = "Ex",
                    CMENU_NAME = "Example",
                    CPARENT_SUB_MENU_ID = "G001",
                    CSUB_MENU_ACCESS = "A,U,D,P,V",
                    CSUB_MENU_ID = "SAB01300",
                    CSUB_MENU_NAME = "Navigator and Batch",
                    CSUB_MENU_TYPE = "P",
                    IFAVORITE_INDEX = 0,
                    IGROUP_INDEX = 0
                });

                loResult.Add(new MenuListDTO
                {
                    CMENU_ID = "Ex",
                    CMENU_NAME = "Example",
                    CPARENT_SUB_MENU_ID = "G001",
                    CSUB_MENU_ACCESS = "A,U,D,P,V",
                    CSUB_MENU_ID = "SAB01500",
                    CSUB_MENU_NAME = "Navigator and Navigator",
                    CSUB_MENU_TYPE = "P",
                    IFAVORITE_INDEX = 0,
                    IGROUP_INDEX = 0
                });

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
