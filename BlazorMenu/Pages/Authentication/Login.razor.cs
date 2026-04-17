using BlazorClientHelper;
using BlazorMenu.Authentication;
using BlazorMenu.Constants;
using BlazorMenuModel.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.Constants;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Interfaces;
using R_CommonFrontBackAPI;

namespace BlazorMenu.Pages.Authentication
{
    public partial class Login
    {
        [Inject] private AuthenticationStateProvider _stateProvider { get; set; }
        [Inject] private R_ILocalStorage _localStorageService { get; set; } = default!;
        [Inject] private R_ToastService _toastService { get; set; } = default!;
        [Inject] private R_PreloadService _preloadService { get; set; } = default!;
        [Inject] private IClientHelper _clientHelper { get; set; } = default!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] private R_IAssetRepository AssetRepository { get; set; } = default!;

        private LoginRequest _loginRequest = new();

        private string _captcha = string.Empty;
        private int _captchaLength = 4;
        private string validateCaptcha = string.Empty;
        private string _loginBgStyle = $"background-image:url(assets/img/bg-illustration.png); background-size:cover; background-position:right center;";
        private string _loginLogoUrl = "assets/img/logo-bimasakti.png";

        protected override async Task OnParametersSetAsync()
        {
            var loEx = new R_Exception();

            try
            {
                var currentCulture = await _localStorageService.GetItemAsync<string>(StorageConstants.Culture);
                if (string.IsNullOrWhiteSpace(currentCulture))
                {
                    await _localStorageService.SetItemAsync<string>(StorageConstants.Culture, AppConstants.DefaultCulture);
                    currentCulture = AppConstants.DefaultCulture;
                }

                if (!currentCulture.Equals(AppConstants.DefaultCulture, StringComparison.InvariantCultureIgnoreCase))
                {
                    var leLoginCulture = R_Culture.R_GetCultureEnum(AppConstants.DefaultCulture);
                    _clientHelper.Set_CultureUI(leLoginCulture);

                    await _localStorageService.SetItemAsync<string>(StorageConstants.Culture, AppConstants.DefaultCulture);
                }

                await _preloadService.Show();
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
                await _preloadService.Hide();
                if (loEx.HasError)
                    _toastService.Error(loEx.ErrorList[0].ErrDescp);
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync(JsConstants.ChangeThemeToggle, "themeControlToggle");
            }
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
                await _preloadService.Hide();
            }

            if (loEx.HasError)
            {
                _toastService.Error(loEx.ErrorList[0].ErrDescp);
            }
        }
    }
}
