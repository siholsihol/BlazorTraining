using BlazorMenuCommon.DTOs;
using BlazorMenuModel;
using R_BlazorFrontEnd.Exceptions;
using R_SecurityPolicyCommon.DTOs;

namespace BlazorMenu.Managers.Menu
{
    public class MenuManager : IMenuManager
    {
        private R_MenuModel _menuModel;

        public MenuManager()
        {
            _menuModel = new R_MenuModel();
        }

        public async Task<GetSecurityPolicyDTO> GetSecurityPolicyAsync(GetSecurityPolicyParameterDTO poParameter)
        {
            var loEx = new R_Exception();
            GetSecurityPolicyDTO result = default!;

            try
            {
                var securityProfile = await _menuModel.GetSecurityPolicyAsync(poParameter);

                result = securityProfile.Data!;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return result;
        }

        public async Task<InfoDTO> GetInfoAsync(GetInfoParameterDTO poParameter)
        {
            var loEx = new R_Exception();
            InfoDTO result = default!;

            try
            {
                var info = await _menuModel.GetInfoAsync(poParameter);

                result = info.Data!;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return result;
        }

        public async Task<List<MenuListDTO>> GetMenuAsync(GetMenuParameterDTO poParameter)
        {
            var loEx = new R_Exception();
            List<MenuListDTO> result = default!;

            try
            {
                var menus = await _menuModel.GetMenuAsync(poParameter);

                result = menus.Data!;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return result;
        }

        public async Task SetFavoriteAsync(FavoriteParameterDTO poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                await _menuModel.SetFavoriteAsync(poParameter);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task SetUnfavoriteAsync(FavoriteParameterDTO poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                await _menuModel.SetUnfavoriteAsync(poParameter);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task SetUserProgramSequenceAsync(SetUserProgramSequenceParameterDTO poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                await _menuModel.SetUserProgramSequenceAsync(poParameter);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}
