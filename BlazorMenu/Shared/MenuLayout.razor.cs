using BlazorClientHelper;
using BlazorMenu.Authentication;
using BlazorMenu.Pages;
using BlazorMenu.Services;
using BlazorMenu.Shared.Drawer;
using BlazorMenu.Shared.Modals;
using BlazorMenu.Shared.Tabs;
using BlazorMenuCommon.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.Helpers;
using R_BlazorFrontEnd.Controls.MessageBox;
using Telerik.Blazor.Components;

namespace BlazorMenu.Shared
{
    public partial class MenuLayout : ComponentBase
    {
        [Inject] private AuthenticationStateProvider _stateProvider { get; set; }
        [Inject] private R_IMenuService _menuService { get; set; }
        [Inject] private MenuTabSetTool TabSetTool { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] private IClientHelper _clientHelper { get; set; }
        [Inject] private R_MessageBoxService _messageBoxService { get; set; }
        [Inject] private R_PreloadService _preloadService { get; set; }
        [Inject] private R_ToastService _toastService { get; set; }

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

        private List<SearchBoxItem> searchBoxData
        {
            get
            {
                var loData = _menuList.Where(x => x.CSUB_MENU_TYPE == "P").
                    Select(x => new SearchBoxItem
                    {
                        Id = x.CSUB_MENU_ID,
                        Text = x.CSUB_MENU_ID + " - " + x.CSUB_MENU_NAME
                    }).ToList();

                return loData;
            }
        }

        protected string NotificationCssClass;

        private bool _notificationOpened = false;
        //private List<BlazorMenuNotificationDTO> _newNotificationMessages = new List<BlazorMenuNotificationDTO>();
        //private List<BlazorMenuNotificationDTO> _oldNotificationMessages = new List<BlazorMenuNotificationDTO>();
        private DotNetObjectReference<MenuLayout> DotNetReference { get; set; }

        private TelerikAutoComplete<SearchBoxItem> TelerikAutoCompleteRef;
        private string _autoCompleteId = IdGeneratorHelper.Generate("searchbox", 3);

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
                await JSRuntime.InvokeVoidAsync("handleNavbarVerticalCollapsed");

                DotNetReference = DotNetObjectReference.Create(this);
                await JSRuntime.InvokeVoidAsync("blazorMenuBootstrap.observeElement", "navbarDropdownNotification", DotNetReference);

                await JSRuntime.InvokeVoidAsync("blazorMenuBootstrap.changeThemeToggle", "themeControlToggle");

                await JSRuntime.InvokeVoidAsync("blazorMenuBootstrap.overrideDefaultKey", DotNetReference);

                await JSRuntime.InvokeVoidAsync("blazorMenuBootstrap.attachFocusHandler", DotNetReference, _autoCompleteId);
                // OpenComponent();
            }
        }

        [JSInvokable("DefaultKeyDown")]
        public Task DefaultKeyDown(KeyboardEventArgs args)
        {
            //var documentationUrl = GetDocumentationBaseUrl();
            //var currentUrl = new Uri(new Uri(documentationUrl), ParseProgramId());

            //await JSRuntime.InvokeVoidAsync("blazorMenuBootstrap.blazorOpen", new object[2] { currentUrl, "_blank" });
            return Task.CompletedTask;
        }

        [JSInvokable("FindKeyDown")]
        public async Task FindKeyDown(KeyboardEventArgs args)
        {
            await TelerikAutoCompleteRef.FocusAsync();
        }

        [JSInvokable("OpenComponent")]
        public void OpenComponent()
        {
            TelerikAutoCompleteRef.Open();
        }

        [JSInvokable("ObserverNotification")]
        public Task ObserverNotification(bool plShow)
        {
            //if (plShow && !_notificationOpened)
            //{
            //    foreach (var message in _newNotificationMessages)
            //    {
            //        message.IsRead = true;
            //    }

            //    _oldNotificationMessages.AddRange(_newNotificationMessages);
            //    _newNotificationMessages.Clear();

            //    _notificationOpened = true;
            //}

            return Task.CompletedTask;
        }

        private async Task OnClickProgram(DrawerMenuItem drawerMenuItem)
        {
            await OnClickProgram(drawerMenuItem.Text, drawerMenuItem.Id);
        }

        private async Task OnClickProgram(string text, string id)
        {
            try
            {
                await TabSetTool.AddTab(text, id, "A,U,D,P,V");
            }
            catch (Exception ex)
            {
                await _messageBoxService.Show(ex.Message);
            }
        }

        private async Task SearchTextValueChanged(object value)
        {
            if (string.IsNullOrWhiteSpace(_searchText))
                return;

            var programId = _searchText.Split(" - ")[0];
            var menuItem = _menuList.FirstOrDefault(x => x.CSUB_MENU_ID == programId);
            if (menuItem != null)
            {
                await OnClickProgram(menuItem.CSUB_MENU_NAME, menuItem.CSUB_MENU_ID);
                _searchText = "";
                TelerikAutoCompleteRef.Close();
            }
        }

        private async Task Logout()
        {
            _preloadService.Show();

            await ((BlazorMenuAuthenticationStateProvider)_stateProvider).MarkUserAsLoggedOut();

            _navigationManager.NavigateTo("/");

            _preloadService.Hide();
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
        private string _modalProfileId = IdGeneratorHelper.Generate("modalprofile");

        private async Task ShowProfilePage()
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("CloseModalTask", async (bool isUpdated) => await OnCloseModalProfilePageTask(isUpdated));

            await modalProfilePage.ShowAsync<Profile>(parameters: parameters);
        }

        private async Task OnCloseModalProfilePageTask(bool isUpdated)
        {
            await modalProfilePage.HideAsync();

            if (isUpdated)
                _toastService.Success("Success update user info.");
        }

        #endregion

        #region Info Page

        private MenuModal modalInfoPage;
        private string _modalInfo = IdGeneratorHelper.Generate("modalinfo");

        private async Task ShowInfoPage()
        {
            await modalInfoPage.ShowAsync<Info>();
        }

        #endregion

        private async Task onkeypress(KeyboardEventArgs eventArgs)
        {
            if (eventArgs.Code == "Enter" && !string.IsNullOrWhiteSpace(_searchText))
            {
                var programId = _searchText.Split(" - ")[0];
                var menuItem = _menuList.FirstOrDefault(x => x.CSUB_MENU_ID == programId);
                if (menuItem != null)
                {
                    await OnClickProgram(menuItem.CSUB_MENU_NAME, menuItem.CSUB_MENU_ID);
                    _searchText = "";
                    TelerikAutoCompleteRef.Close();
                }
            }
        }

        //#region Documentation

        ////private string GetDocumentationBaseUrl()
        ////{
        ////    var lcUrl = _configuration.GetSection("R_ServiceUrlSection:R_DocumentationServiceUrl").Get<string>();

        ////    return lcUrl;
        ////}

        ////private string ParseProgramId()
        ////{
        ////    var relativeUri = _navigationManager.ToBaseRelativePath(_navigationManager.Uri).Replace("#", "");

        ////    if (relativeUri.IndexOf('?') > -1)
        ////    {
        ////        relativeUri = relativeUri.Substring(0, relativeUri.IndexOf('?'));
        ////    }

        ////    var urlSegment = relativeUri.Split("/");
        ////    if (urlSegment.Count() > 1)
        ////    {
        ////        relativeUri = urlSegment.Last();

        ////        var programName = _menuList.FirstOrDefault(x => x.CSUB_MENU_ID == relativeUri).CSUB_MENU_NAME;
        ////        relativeUri = DocumentationTemplateParser.ParseTemplate(relativeUri, programName);
        ////    }

        ////    return relativeUri;
        ////}

        //#endregion
    }

    public class SearchBoxItem
    {
        public string Id { get; set; }
        public string Text { get; set; }
    }
}
