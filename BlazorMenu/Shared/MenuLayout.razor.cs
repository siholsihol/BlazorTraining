﻿using BlazorClientHelper;
using BlazorMenu.Authentication;
using BlazorMenu.Pages;
using BlazorMenu.Services;
using BlazorMenu.Shared.Drawer;
using BlazorMenu.Shared.Modals;
using BlazorMenu.Shared.Tabs;
using BlazorMenuCommon.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace BlazorMenu.Shared
{
    public partial class MenuLayout : ComponentBase
    {
        [Inject] private AuthenticationStateProvider _stateProvider { get; set; }
        [Inject] private R_IMenuService _menuService { get; set; }
        [Inject] private MenuTabSetTool TabSetTool { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] private IClientHelper _clientHelper { get; set; }

        private List<MenuListDTO> _menuList = new();
        private List<DrawerMenuItem> _data = new();
        private string _searchText = string.Empty;
        private string _userId = string.Empty;
        private List<DrawerMenuItem> _filteredData
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_searchText))
                    return new List<DrawerMenuItem>();

                var loData = _menuList.Where(x => x.CSUB_MENU_TYPE == "P" &&
                (x.CSUB_MENU_ID.ToLower().Contains(_searchText.ToLower())) || x.CSUB_MENU_NAME.ToLower().Contains(_searchText.ToLower())).
                    Select(x => new DrawerMenuItem
                    {
                        Id = x.CSUB_MENU_ID,
                        Text = x.CSUB_MENU_NAME,
                        Level = 2
                    }).ToList();

                return loData;
            }
        }

        protected string NotificationCssClass;

        private bool _notificationOpened = false;
        //private List<BlazorMenuNotificationDTO> _newNotificationMessages = new List<BlazorMenuNotificationDTO>();
        //private List<BlazorMenuNotificationDTO> _oldNotificationMessages = new List<BlazorMenuNotificationDTO>();
        private DotNetObjectReference<MenuLayout> DotNetReference { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _menuList = await _menuService.GetMenuAsync();

                var menuIds = _menuList.Where(x => x.CMENU_ID != "FAV")
                    .GroupBy(x => x.CMENU_ID)
                    .Select(x => x.First()).Select(x => x.CMENU_ID).ToArray();

                _data = menuIds.Select(id => new DrawerMenuItem
                {
                    Id = id,
                    Text = _menuList.FirstOrDefault(x => x.CMENU_ID == id).CMENU_NAME,
                    Level = 0,
                    Children = _menuList.Where(x => x.CSUB_MENU_TYPE == "G" && x.CMENU_ID == id).Select(y => new DrawerMenuItem
                    {
                        Id = y.CSUB_MENU_ID,
                        Text = y.CSUB_MENU_NAME,
                        Level = 1,
                        Children = _menuList.Where(z => z.CSUB_MENU_TYPE == "P" && z.CPARENT_SUB_MENU_ID == y.CSUB_MENU_ID && z.CMENU_ID == id).Select(yy => new DrawerMenuItem
                        {
                            Id = yy.CSUB_MENU_ID,
                            Text = yy.CSUB_MENU_NAME,
                            Level = 2,
                            Children = new()
                        }).ToList()
                    }).ToList()
                }).ToList();

                //var lcUserId = _clientHelper.UserId.ToUpper();
                //if (lcUserId.Length > 3)
                //    lcUserId = lcUserId.Substring(0, 3);

                _userId = "TR";
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //await JSRuntime.InvokeVoidAsync("handleNavbarVerticalCollapsed");

                await JSRuntime.InvokeVoidAsync("searchInit");

                DotNetReference = DotNetObjectReference.Create(this);
                await JSRuntime.InvokeVoidAsync("blazorMenuBootstrap.observeElement", "navbarDropdownNotification", DotNetReference);

                await JSRuntime.InvokeVoidAsync("blazorMenuBootstrap.changeThemeToggle", "themeControlToggle");
            }
        }

        private void OnClickProgram(DrawerMenuItem drawerMenuItem)
        {
            TabSetTool.AddTab(drawerMenuItem.Text, drawerMenuItem.Id, "A,U,D,P,V");
        }

        private async Task Logout()
        {
            await ((BlazorMenuAuthenticationStateProvider)_stateProvider).MarkUserAsLoggedOut();

            _navigationManager.NavigateTo("/");
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            if (DotNetReference != null)
            {
                DotNetReference.Dispose();
            }
        }

        #region ProfilePage

        private MenuModal modalProfilePage;

        private async Task ShowProfilePage()
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("CloseModalTask", OnCloseModalProfilePageTask);

            await modalProfilePage.ShowAsync<Profile>(parameters: parameters);
        }

        private async Task OnCloseModalProfilePageTask(bool isUpdated)
        {
            await modalProfilePage.HideAsync();

            //if (isUpdated)
            //    _toastService.Success("Success update user info.");
        }

        #endregion

        #region Info Page

        private MenuModal modalInfoPage;

        private async Task ShowInfoPage()
        {
            await modalInfoPage.ShowAsync<Info>();
        }

        #endregion
    }
}
