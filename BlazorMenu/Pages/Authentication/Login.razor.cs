using BlazorMenu.Authentication;
using BlazorMenu.Constants;
using BlazorMenuModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Interfaces;

namespace BlazorMenu.Pages.Authentication
{
    public partial class Login
    {
        [Inject] private AuthenticationStateProvider _stateProvider { get; set; }
        [Inject] private R_ILocalStorage _localStorageService { get; set; }
        [Inject] private R_ToastService _toastService { get; set; }
        [Inject] private R_PreloadService _preloadService { get; set; }

        private readonly R_LoginViewModel _loginVM = new();
        private string _captcha = "";
        private int _captchaLength = 4;
        private string validateCaptcha;

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
                await _localStorageService.SetItemAsync(StorageConstants.IsLogin, true);

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
            finally
            {
                _preloadService.Hide();
            }

            if (loEx.HasError)
            {
                _toastService.Error(loEx.ErrorList[0].ErrDescp);
            }
        }
    }
}
