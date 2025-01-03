﻿using BlazorClientHelper;
using BlazorMenu.Authentication;
using BlazorMenu.Services;
using BlazorMenu.Shared.Tabs;
using BlazorMenuModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using R_AuthenticationEnumAndInterface;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_BlazorFrontEnd.Controls.Notification;
using R_BlazorFrontEnd.Exceptions;
using R_CrossPlatformSecurity;

namespace BlazorMenu.Pages.Authentication
{
    public partial class Login
    {
        [Inject] private AuthenticationStateProvider _stateProvider { get; set; }
        [Inject] private R_ITokenRepository _tokenRepository { get; set; }
        [Inject] private BlazorMenuLocalStorageService _localStorageService { get; set; }
        [Inject] private IClientHelper _clientHelper { get; set; }
        [Inject] public R_MessageBoxService R_MessageBox { get; set; }
        [Inject] private R_ISymmetricJSProvider _encryptProvider { get; set; }
        [Inject] private MenuTabSetTool MenuTabSetTool { get; set; }
        [Inject] private R_NotificationService _notificationService { get; set; }

        private readonly R_LoginViewModel _loginVM = new();
        private string _captcha = "";
        private int _captchaLength = 4;

        protected override async Task OnParametersSetAsync()
        {
            var loEx = new R_Exception();

            try
            {
                //var state = await _stateProvider.GetAuthenticationStateAsync();
                //if (state.User.Identity.IsAuthenticated)
                //    _navigationManager.NavigateTo("/");

                //var loPolicyParameter = await loClientWrapper.R_GetSecurityPolicyParameterAsync();

                //_loginModel.CompanyId = "rcd";
                //_loginModel.UserId = "cp";
                //_loginModel.Password = "cp";
            }
            catch (R_Exception rex)
            {
                loEx.Add(rex);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            if (loEx.HasError)
                _notificationService.Error(loEx.ErrorList[0].ErrDescp);
        }

        protected override void OnInitialized()
        {
            _captcha = R_BlazorFrontEnd.Controls.Captcha.Tools.GetCaptchaWord(_captchaLength);

            base.OnInitialized();
        }

        private async Task ValidateUser()
        {
            var loEx = new R_Exception();

            try
            {
                //_clientHelper.Set_ComputerId();
                //_clientHelper.Set_CompanyId(_loginModel.CompanyId);

                //var lcEncryptedPassword = await _encryptProvider.TextEncrypt(_loginModel.Password, _loginModel.UserId);

                //var loPolicyLogin = await loClientWrapper.R_SecurityPolicyLogonAsync
                //    (
                //        new R_SecurityPolicyLogonRequest
                //        {
                //            CCOMPANY_ID = _loginModel.CompanyId,
                //            CUSER_ID = _loginModel.UserId.ToLower(),
                //            CUSER_PASSWORD = lcEncryptedPassword
                //        }
                //    );

                //if (loPolicyLogin.Data != null)
                //{
                //    _tokenRepository.R_SetToken(loPolicyLogin.Data.CTOKEN);
                //    _tokenRepository.R_SetRefreshToken(loPolicyLogin.Data.CREFRESH_TOKEN);

                //    //await _localStorageService.SetItemAsStringAsync(StorageConstants.TokenId, loPolicyLogin.Data.CTOKEN_ID);

                //    R_LoginViewModel _loginViewModel = new R_LoginViewModel();
                //    var loLogin = await _loginViewModel.LoginAsync(_loginModel);

                //    _clientHelper.Set_UserId(loLogin.CUSER_ID);
                //    _clientHelper.Set_UserName(loLogin.CUSER_NAME);

                //    //await _menuService.SetMenuAccessAsync();

                //    if (!string.IsNullOrWhiteSpace(loLogin.CCULTURE_ID))
                //    {
                //        var leLoginCulture = R_Culture.R_GetCultureEnum(loLogin.CCULTURE_ID);

                //        _clientHelper.Set_CultureUI(leLoginCulture);
                //    }
                //    else
                //        _clientHelper.Set_CultureUI(eCulture.English);

                //    var loCultureInfoBuilder = new CultureInfoBuilder();
                //    loCultureInfoBuilder.WithNumberFormatInfo(loLogin.CNUMBER_FORMAT, loLogin.IDECIMAL_PLACES)
                //                        .WithDatePattern(loLogin.CDATE_LONG_FORMAT, loLogin.CDATE_SHORT_FORMAT)
                //                        .WithTimePattern(loLogin.CTIME_LONG_FORMAT, loLogin.CTIME_SHORT_FORMAT);

                //    var loCultureInfo = loCultureInfoBuilder.BuildCultureInfo();

                //    _clientHelper.Set_Culture(loCultureInfo.NumberFormat, loCultureInfo.DateTimeFormat);

                //    await _localStorageService.SetCultureAsync(loLogin.CCULTURE_ID);

                //    var loDictCulture = new Dictionary<string, string>
                //    {
                //        { "CNUMBER_FORMAT", loLogin.CNUMBER_FORMAT },
                //        { "IDECIMAL_PLACES", loLogin.IDECIMAL_PLACES.ToString() },
                //        { "CDATE_LONG_FORMAT", loLogin.CDATE_LONG_FORMAT },
                //        { "CDATE_SHORT_FORMAT", loLogin.CDATE_SHORT_FORMAT },
                //        { "CTIME_LONG_FORMAT", loLogin.CTIME_LONG_FORMAT },
                //        { "CTIME_SHORT_FORMAT", loLogin.CTIME_SHORT_FORMAT }
                //    };
                //    await _localStorageService.SetCultureInfoAsync(loDictCulture);

                //    if (!loLogin.CCULTURE_ID.Equals("en", StringComparison.InvariantCultureIgnoreCase))
                //        _navigationManager.NavigateTo(_navigationManager.Uri, true);

                //    MenuTabSetTool.Tabs.Clear();

                //    await ((BlazorMenuAuthenticationStateProvider)_stateProvider).MarkUserAsAuthenticated();
                //}
                //else
                //{
                //    throw new Exception("Invalid username or password");
                //}

                await _localStorageService.SetIsLoginAsync(true);

                await ((BlazorMenuAuthenticationStateProvider)_stateProvider).MarkUserAsAuthenticated();
            }
            catch (R_Exception rex)
            {
                loEx.Add(rex);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            if (loEx.HasError)
            {
                _notificationService.Error(loEx.ErrorList[0].ErrDescp);
            }
        }
    }
}
