using BlazorMenuCommon.DTOs;
using R_BlazorFrontEnd.Exceptions;

namespace BlazorMenu.Services
{
    public class R_MenuService : R_IMenuService
    {
        public Dictionary<string, string[]> MenuAccess { get; private set; }

        public string[] MenuIdList { get; private set; }

        public Task SetMenuAccessAsync()
        {
            var loEx = new R_Exception();
            List<MenuProgramAccessDTO> loResult = null;

            try
            {
                loResult = new List<MenuProgramAccessDTO>();

                loResult.Add(new MenuProgramAccessDTO
                {
                    CPROGRAM_ID = "SAB00600",
                    CACCESS_ID = "A,U,D,P,V"
                });

                loResult.Add(new MenuProgramAccessDTO
                {
                    CPROGRAM_ID = "SAB00700",
                    CACCESS_ID = "A,U,D,P,V"
                });

                loResult.Add(new MenuProgramAccessDTO
                {
                    CPROGRAM_ID = "SAB00900",
                    CACCESS_ID = "A,U,D,P,V"
                });

                //loResult.Add(new MenuProgramAccessDTO
                //{
                //    CPROGRAM_ID = "SAB01300",
                //    CACCESS_ID = "A,U,D,P,V"
                //});

                //loResult.Add(new MenuProgramAccessDTO
                //{
                //    CPROGRAM_ID = "SAB01400",
                //    CACCESS_ID = "A,U,D,P,V"
                //});

                //loResult.Add(new MenuProgramAccessDTO
                //{
                //    CPROGRAM_ID = "SAB00100",
                //    CACCESS_ID = "A,U,D,P,V"
                //});

                //loResult.Add(new MenuProgramAccessDTO
                //{
                //    CPROGRAM_ID = "SAB00200",
                //    CACCESS_ID = "A,U,D,P,V"
                //});

                loResult.Add(new MenuProgramAccessDTO
                {
                    CPROGRAM_ID = "SAB01000",
                    CACCESS_ID = "A,U,D,P,V"
                });

                loResult.Add(new MenuProgramAccessDTO
                {
                    CPROGRAM_ID = "SAB02400",
                    CACCESS_ID = "A,U,D,P,V"
                });

                MenuAccess = loResult.ToDictionary(x => x.CPROGRAM_ID, x => x.CACCESS_ID.Split(','));
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return Task.CompletedTask;
        }

        public void CreateControlsProgram(List<MenuListDTO> poExistingList)
        {
            poExistingList.Add(new MenuListDTO
            {
                CMENU_ID = "CT",
                CMENU_NAME = "Controls",
                CPARENT_SUB_MENU_ID = "CT",
                CSUB_MENU_ACCESS = "A,U,D,P,V",
                CSUB_MENU_ID = "G002",
                CSUB_MENU_NAME = "Tab",
                CSUB_MENU_TYPE = "G",
                IFAVORITE_INDEX = 0,
                IGROUP_INDEX = 2
            });

            poExistingList.Add(new MenuListDTO
            {
                CMENU_ID = "CT",
                CMENU_NAME = "Controls",
                CPARENT_SUB_MENU_ID = "G002",
                CSUB_MENU_ACCESS = "A,U,D,P,V",
                CSUB_MENU_ID = "SAB03000",
                CSUB_MENU_NAME = "Tab",
                CSUB_MENU_TYPE = "P",
                IFAVORITE_INDEX = 0,
                IGROUP_INDEX = 0
            });
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

                CreateControlsProgram(loResult);
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
