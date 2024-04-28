using BlazorMenuCommon;
using BlazorMenuCommon.DTOs;
using R_APIClient;
using R_BlazorFrontEnd.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorMenuModel
{
    public class R_MenuModel : IBlazorMenu
    {
        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrl";
        private const string DEFAULT_SERVICEPOINT_NAME = "api/BlazorMenu";

        #region GetMenuAccess
        public BlazorMenuResultDTO<List<MenuProgramAccessDTO>> GetMenuAccess(GetMenuAccessParameterDTO poParameter)
        {
            throw new NotImplementedException();
        }

        public async Task<BlazorMenuResultDTO<List<MenuProgramAccessDTO>>> GetMenuAccessAsync(GetMenuAccessParameterDTO poParameter)
        {
            var loEx = new R_Exception();
            BlazorMenuResultDTO<List<MenuProgramAccessDTO>> loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<BlazorMenuResultDTO<List<MenuProgramAccessDTO>>, GetMenuAccessParameterDTO>
                    (DEFAULT_SERVICEPOINT_NAME,
                    nameof(IBlazorMenu.GetMenuAccess),
                    poParameter,
                    "",
                    false,
                    false);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
        #endregion

        #region GetMenu
        public BlazorMenuResultDTO<List<MenuListDTO>> GetMenu(GetMenuParameterDTO poParameter)
        {
            throw new NotImplementedException();
        }

        public async Task<BlazorMenuResultDTO<List<MenuListDTO>>> GetMenuAsync(GetMenuParameterDTO poParameter)
        {
            var loEx = new R_Exception();
            BlazorMenuResultDTO<List<MenuListDTO>> loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<BlazorMenuResultDTO<List<MenuListDTO>>, GetMenuParameterDTO>
                    (DEFAULT_SERVICEPOINT_NAME,
                    nameof(IBlazorMenu.GetMenu),
                    poParameter,
                    "",
                    false,
                    false);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
        #endregion

        #region GetInfo
        public BlazorMenuResultDTO<InfoDTO> GetInfo(GetInfoParameterDTO poParameter)
        {
            throw new NotImplementedException();
        }

        public async Task<BlazorMenuResultDTO<InfoDTO>> GetInfoAsync(GetInfoParameterDTO poParameter)
        {
            var loEx = new R_Exception();
            BlazorMenuResultDTO<InfoDTO> loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<BlazorMenuResultDTO<InfoDTO>, GetInfoParameterDTO>
                    (DEFAULT_SERVICEPOINT_NAME,
                    nameof(IBlazorMenu.GetInfo),
                    poParameter,
                    "",
                    false,
                    false);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
        #endregion

        #region GetUserProfile
        public BlazorMenuResultDTO<GetUserProfileDTO> GetUserProfile(GetUserProfileParameterDTO poParameter)
        {
            throw new NotImplementedException();
        }

        public async Task<BlazorMenuResultDTO<GetUserProfileDTO>> GetUserProfileAsync(GetUserProfileParameterDTO poParameter)
        {
            var loEx = new R_Exception();
            BlazorMenuResultDTO<GetUserProfileDTO> loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<BlazorMenuResultDTO<GetUserProfileDTO>, GetUserProfileParameterDTO>
                    (DEFAULT_SERVICEPOINT_NAME,
                    nameof(IBlazorMenu.GetUserProfile),
                    poParameter,
                    "",
                    false,
                    false);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
        #endregion
    }
}
