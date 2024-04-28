using BlazorMenuCommon.DTOs;
using BlazorMenuModel.Models;
using R_BlazorFrontEnd.Exceptions;
using R_SecurityPolicyCommon.Responses;
using R_SecurityPolicyModel;
using System;
using System.Threading.Tasks;

namespace BlazorMenuModel
{
    public class R_LoginViewModel
    {
        public LoginModel LoginModel { get; set; } = new LoginModel();
        public R_GetSecurityPolicyParameterResponse SecurityPolicyParameter { get; set; }
        public LoginDTO LoginResult { get; set; }

        private R_LoginModel _loginModel = new R_LoginModel();
        private R_SecurityModel _securityModel = new R_SecurityModel();

        public async Task LoginAsync(string pcUserPassword)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = new LogonParamaterDTO
                {
                    CCOMPANY_ID = LoginModel.CompanyId,
                    CUSER_ID = LoginModel.UserId,
                    CUSER_PASSWORD = pcUserPassword,
                    CAPPLICATION_ID = "BIMASAKTI"
                };

                var loLogin = await _loginModel.LogonAsync(loParam);

                LoginResult = loLogin.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task UserLockingFlushAsync(UserLockingFlushParameterDTO poParam)
        {
            var loEx = new R_Exception();

            try
            {
                await _loginModel.UserLockingFlushAsync(poParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task R_GetSecurityPolicyParameterAsync()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _securityModel.R_GetSecurityPolicyParameterAsync();

                SecurityPolicyParameter = loResult.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}
