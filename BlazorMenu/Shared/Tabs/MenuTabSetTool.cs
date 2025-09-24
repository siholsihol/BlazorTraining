using BlazorMenu.Constants;
using BlazorMenu.Resources;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Configurations;
using R_BlazorFrontEnd.Controls.Router;
using R_BlazorFrontEnd.Deployment.Interfaces;
using R_BlazorFrontEnd.Helpers;
using R_BlazorFrontEnd.Interfaces;

namespace BlazorMenu.Shared.Tabs
{
    public class MenuTabSetTool
    {
        private readonly NavigationManager _navigationManager;
        private readonly R_ITenant? _tenant;
        private readonly RouteManager _routeManager;
        private readonly IConfiguration _configuration;
        private readonly R_IAssemblyDataProvider _assemblyDataProvider;

        public MenuTabSetTool(
            NavigationManager navigationManager,
            RouteManager routeManager,
            IConfiguration configuration,
            //IServiceProvider serviceProvider,
            R_IAssemblyDataProvider assemblyDataProvider
        )
        {
            _navigationManager = navigationManager;
            _routeManager = routeManager;
            _configuration = configuration;
            _assemblyDataProvider = assemblyDataProvider;
            //_tenant = serviceProvider.GetTenantFromService();
        }

        public List<MenuTab> Tabs { get; set; } = new();
        public int ActiveTabIndex = 0;

        public async Task AddTab(string title, string url, string pcAccess = "")
        {
            try
            {
                var lcDeploymentUrl = GetDeploymentServiceUrl();

                if (!string.IsNullOrWhiteSpace(lcDeploymentUrl))
                {
                    var newRelativeUri = $"{url}Front";

                    await _assemblyDataProvider.CheckAssemblyAsync(newRelativeUri);
                }
                else
                {
                    var llExistRoute = _routeManager.Routes?.Any(x => x.UriSegments.Any(y => string.Equals(y, url, StringComparison.InvariantCultureIgnoreCase)));
                    if (!llExistRoute.HasValue || !llExistRoute.Value)
                        throw new Exception(String.Format(R_FrontUtility.R_GetMessage(typeof(BlazorMenuLocalizer), "MenuTab_E001", pcResourceName: "BlazorMenuResources"), url));
                }

                SetAllTabInactive();

                var lcAccess = string.IsNullOrWhiteSpace(pcAccess) ? "V" : pcAccess;

                var loNewTab = new MenuTab
                {
                    Url = url,
                    Title = title,
                    IsActive = true,
                    Access = lcAccess
                };

                Tabs.Add(loNewTab);

                _navigationManager.NavigateTo(GetUrl(url));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void NavigateToTab(string title, string url)
        {
            SetAllTabInactive();

            var selTab = GetSelectedTab(title, url);
            if (selTab is not null)
            {
                if (string.IsNullOrEmpty(selTab.Title))
                    selTab.Title = title;

                selTab.IsActive = true;
            }

            _navigationManager.NavigateTo(GetUrl(url));
        }

        private string GetUrl(string url)
        {
            var lcUrlTenant = url;
            if (_tenant is not null)
                lcUrlTenant = _tenant.Identifier + "/" + url;
            return lcUrlTenant;
        }

        private void SetAllTabInactive()
        {
            Tabs.ForEach(x =>
            {
                x.IsActive = false;
            });
        }
        private MenuTab? GetSelectedTab(string title, string url)
        {
            return Tabs.FirstOrDefault(m => m.Url == url && (m.Title == title || string.IsNullOrEmpty(m.Title)));
        }

        private string? GetDeploymentServiceUrl()
        {
            var lcUrl = string.Empty;

            try
            {
                lcUrl = R_FrontConfig.R_GetConfigAsString(ConfigNames.DeploymentServiceUrl);
            }
            catch (Exception)
            {
            }

            return lcUrl;
        }
    }
}
