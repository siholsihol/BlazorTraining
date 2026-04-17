using BlazorClientHelper;
using BlazorMenu.Authentication;
using BlazorMenu.Constants;
using BlazorMenu.Services;
using BlazorMenu.Shared;
using BlazorMenu.Shared.Tabs;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using R_BlazorFrontEnd.Controls.Menu;
using R_BlazorFrontEnd.Interfaces;
using System.Globalization;

namespace BlazorMenu.Extensions
{
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection R_AddBlazorMenuServices(this IServiceCollection services)
        {
            services.AddAuthorizationCore();

            services.AddSingleton<R_IAssetRepository, R_AssetRepository>();

            services.AddScoped<AuthenticationStateProvider, BlazorMenuAuthenticationStateProvider>();

            services.AddSingleton(typeof(R_ILocalizer<>), typeof(R_Localizer<>));
            services.AddSingleton<R_ILocalizer, R_Localizer>();
            services.AddSingleton<R_IMainBody, MainBody>();

            services.AddScoped<MenuTabSetTool>();

            services.AddTransient<R_IMenuService, R_MenuService>();

            services.AddSingleton<IClientHelper, U_GlobalVar>();

            services.AddScoped<R_ILocalStorage, R_LocalStorage>();
            //services.AddScoped<BlazorMenuLocalStorageService>();

            return services;
        }

        internal static async Task R_UseBlazorMenuServices(this WebAssemblyHost host)
        {
            var loLocalStorage = host.Services.GetRequiredService<R_ILocalStorage>();
            var lcCulture = await loLocalStorage.GetItemAsync<string>(StorageConstants.Culture);

            CultureInfo loCulture = new CultureInfo("en");
            if (!string.IsNullOrWhiteSpace(lcCulture))
                loCulture = new CultureInfo(lcCulture);

            CultureInfo.DefaultThreadCurrentCulture = loCulture;
            CultureInfo.DefaultThreadCurrentUICulture = loCulture;
        }

        internal static IServiceCollection AutoRegisterInterfaces<T>(this IServiceCollection services)
        {
            var @interface = typeof(T);

            var types = @interface
                .Assembly
                .GetExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Select(t => new
                {
                    Service = t.GetInterface($"I{t.Name}"),
                    Implementation = t
                })
                .Where(t => t.Service != null);

            foreach (var type in types)
            {
                if (@interface.IsAssignableFrom(type.Service))
                {
                    services.AddTransient(type.Service, type.Implementation);
                }
            }

            return services;
        }
    }
}
