﻿using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls.Router;

namespace BlazorMenu.Shared.Tabs
{
    public class MenuTabSetTool
    {
        private readonly NavigationManager _navigationManager;
        private readonly RouteManager _routeManager;

        public MenuTabSetTool(
            NavigationManager navigationManager,
            RouteManager routeManager)
        {
            _navigationManager = navigationManager;
            _routeManager = routeManager;
        }

        public List<MenuTab> Tabs { get; set; } = new();
        public int ActiveTabIndex = 0;

        public Task AddTab(string title, string url, string pcAccess = "")
        {
            try
            {
                var llExistRoute = _routeManager.Routes.Any(x => x.UriSegments.Any(y => string.Equals(y, url, StringComparison.InvariantCultureIgnoreCase)));
                if (!llExistRoute)
                    throw new Exception($"{url} not found.");

                Tabs.ForEach(x =>
                {
                    x.IsActive = false;
                });

                MenuTab loNewTab = null;
                var selTab = Tabs.FirstOrDefault(m => m.Url == url && (m.Title == title || string.IsNullOrEmpty(m.Title)));
                if (selTab == null)
                {
                    var lcAccess = string.IsNullOrWhiteSpace(pcAccess) ? "V" : pcAccess;

                    loNewTab = new MenuTab
                    {
                        Url = url,
                        Title = title,
                        IsActive = true,
                        Access = lcAccess
                    };

                    Tabs.Add(loNewTab);
                }
                else
                {
                    if (string.IsNullOrEmpty(selTab.Title))
                        selTab.Title = title;

                    selTab.IsActive = true;
                }

                _navigationManager.NavigateTo(url);
            }
            catch (Exception)
            {
                throw;
            }

            return Task.CompletedTask;
        }
    }
}
