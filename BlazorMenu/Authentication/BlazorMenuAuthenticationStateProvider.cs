using BlazorClientHelper;
using BlazorMenu.Constants.Storage;
using BlazorMenu.Services;
using Microsoft.AspNetCore.Components.Authorization;
using R_AuthenticationEnumAndInterface;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Interfaces;
using R_CommonFrontBackAPI;
using System.Security.Claims;

namespace BlazorMenu.Authentication
{
    public class BlazorMenuAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly R_ILocalStorage _localStorageService;
        private readonly IClientHelper _clientHelper;

        public BlazorMenuAuthenticationStateProvider(
            R_ITokenRepository tokenRepository,
            R_ILocalStorage localStorageService,
            IClientHelper clientHelper,
            R_ITenant tenant)
        {
            _localStorageService = localStorageService;
            _clientHelper = clientHelper;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var loEx = new R_Exception();
            AuthenticationState loState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            try
            {
                _clientHelper.Set_CultureUI(eCulture.English);

                var loCultureInfoBuilder = new CultureInfoBuilder();
                loCultureInfoBuilder.WithNumberFormatInfo(".", 2)
                                    .WithDatePattern("MMMM d, yyyy", "M/d/yyyy")
                                    .WithTimePattern("h:mm:ss tt", "h:mm tt");

                var loCultureInfo = loCultureInfoBuilder.BuildCultureInfo();
                _clientHelper.Set_Culture(loCultureInfo.NumberFormat, loCultureInfo.DateTimeFormat);

                var loClaims = new List<Claim>()
                {
                    new Claim("USER_ID", "training")
                };
                var llIsLogin = await _localStorageService.GetItemAsync<bool>(StorageConstants.IsLogin);
                if (llIsLogin)
                    loState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(loClaims, "jwt")));
            }
            catch (Exception)
            {
                return loState;
            }

            loEx.ThrowExceptionIfErrors();

            return loState;
        }

        public async Task MarkUserAsLoggedOut()
        {
            var loEx = new R_Exception();

            try
            {
                await _localStorageService.RemoveItemAsync(StorageConstants.IsLogin);

                var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
                var authState = Task.FromResult(new AuthenticationState(anonymousUser));

                NotifyAuthenticationStateChanged(authState);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task MarkUserAsAuthenticated()
        {
            var authState = Task.FromResult(await GetAuthenticationStateAsync());

            NotifyAuthenticationStateChanged(authState);
        }
    }
}
