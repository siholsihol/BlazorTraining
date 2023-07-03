using BlazorMenu.Services;
using BlazorMenu.Shared;
using BlazorMenu.Shared.Tabs;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using R_BlazorFrontEnd.Controls.Menu;
using R_BlazorFrontEnd.Interfaces;
using System.Globalization;

namespace BlazorMenu.Extensions
{
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection R_AddBlazorFrontEnd(this IServiceCollection services)
        {
            services.AddSingleton(typeof(R_ILocalizer<>), typeof(R_Localizer<>));
            services.AddSingleton<R_ILocalizer, R_Localizer>();
            services.AddSingleton<R_IMainBody, MainBody>();

            services.AddSingleton<MenuTabSetTool>();

            services.AddSingleton<R_IMenuService, R_MenuService>();

            services.AddScoped<LocalStorageService>();

            return services;
        }

        internal static async Task R_UseBlazorFrontEnd(this WebAssemblyHost host)
        {
            var loLocalStorage = host.Services.GetRequiredService<LocalStorageService>();
            var lcCulture = await loLocalStorage.GetCulture();

            CultureInfo loCulture = new CultureInfo("en");
            if (!string.IsNullOrWhiteSpace(lcCulture))
                loCulture = new CultureInfo(lcCulture);

            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en");

            var loCultureInfoBuilder = new CultureInfoBuilder();
            loCultureInfoBuilder.WithNumberFormatInfo(".", 2);
            loCultureInfoBuilder.WithDatePattern("d MMMM, yyyy", "M/d/yyyy")
                                .WithTimePattern("H:mm:ss", "hh:mm tt");
            var loCultureInfo = loCultureInfoBuilder.BuildCultureInfo();

            var loCultureUI = new CultureInfo("en");
            loCultureUI.NumberFormat = loCultureInfo.NumberFormat;
            loCultureUI.DateTimeFormat = loCultureInfo.DateTimeFormat;
            CultureInfo.DefaultThreadCurrentCulture = loCultureUI;
        }
    }
}
