﻿using BlazorMenu.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Interfaces;
using Toolbelt.Blazor;

namespace BlazorMenu.Services
{
    public class HttpInterceptorService
    {
        private readonly HttpClientInterceptor _httpClientInterceptor;
        private readonly NavigationManager _navigationManager;
        private readonly AuthenticationStateProvider _stateProvider;
        private readonly R_ToastService _toastService;

        public HttpInterceptorService(
            HttpClientInterceptor httpClientInterceptor,
            NavigationManager navigationManager,
            AuthenticationStateProvider stateProvider,
            R_ToastService toastService,
            R_IEnvironment environment)
        {
            _httpClientInterceptor = httpClientInterceptor;
            _navigationManager = navigationManager;
            _stateProvider = stateProvider;
        }

        public void RegisterEvent()
        {
            _httpClientInterceptor.BeforeSendAsync += InterceptBeforeHttpAsync;
            _httpClientInterceptor.AfterSendAsync += InterceptAfterHttpAsync;
        }

        public Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e)
        {
            return Task.CompletedTask;
        }

        public async Task InterceptAfterHttpAsync(object sender, HttpClientInterceptorEventArgs e)
        {
            var absPath = e.Request.RequestUri.AbsolutePath;
            if (absPath.Contains("R_RefreshToken") && !e.Response.IsSuccessStatusCode)
            {
                await ((BlazorMenuAuthenticationStateProvider)_stateProvider).MarkUserAsLoggedOut();
                _navigationManager.NavigateTo("/");

                _toastService.Error("Invalid refresh token");
            }
        }

        public void DisposeEvent() => _httpClientInterceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;
    }
}
