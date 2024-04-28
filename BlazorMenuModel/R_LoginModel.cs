using BlazorMenuCommon;
using BlazorMenuCommon.DTOs;
using R_APIClient;
using R_BlazorFrontEnd.Exceptions;
using System;
using System.Threading.Tasks;

namespace BlazorMenuModel
{
    public class R_LoginModel : IBlazorLogin
    {
        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrl";
        private const string DEFAULT_SERVICEPOINT_NAME = "api/BlazorMenu";

        #region Logon
        public BlazorMenuResultDTO<LoginDTO> Logon(LogonParamaterDTO poParameter)
        {
            throw new NotImplementedException();
        }

        public async Task<BlazorMenuResultDTO<LoginDTO>> LogonAsync(LogonParamaterDTO poParameter)
        {
            var loEx = new R_Exception();
            BlazorMenuResultDTO<LoginDTO> loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;

                loResult = await R_HTTPClientWrapper.R_APIRequestObject<BlazorMenuResultDTO<LoginDTO>, LogonParamaterDTO>(
                    DEFAULT_SERVICEPOINT_NAME,
                    nameof(IBlazorLogin.Logon),
                    poParameter,
                    "",
                    true,
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

        #region UserLockingFlush
        public BlazorMenuResultDTO UserLockingFlush(UserLockingFlushParameterDTO poParameter)
        {
            throw new NotImplementedException();
        }

        public async Task<BlazorMenuResultDTO> UserLockingFlushAsync(UserLockingFlushParameterDTO poParameter)
        {
            var loEx = new R_Exception();
            BlazorMenuResultDTO loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;

                loResult = await R_HTTPClientWrapper.R_APIRequestObject<BlazorMenuResultDTO, UserLockingFlushParameterDTO>(
                    DEFAULT_SERVICEPOINT_NAME,
                    nameof(IBlazorLogin.UserLockingFlush),
                    poParameter,
                    "",
                    true,
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
