using BlazorClientHelper;
using BlazorMenu.Authentication;
using BlazorMenu.Pages;
using BlazorMenu.Services;
using BlazorMenu.Shared.Drawer;
using BlazorMenu.Shared.Modals;
using BlazorMenu.Shared.Overlay;
using BlazorMenu.Shared.Tabs;
using BlazorMenuCommon.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.Constants;
using R_BlazorFrontEnd.Controls.Helpers;
using R_BlazorFrontEnd.Controls.MessageBox;
using Telerik.Blazor.Components;

namespace BlazorMenu.Shared
{
    public partial class MenuLayout : ComponentBase
    {
        [Inject] private AuthenticationStateProvider _stateProvider { get; set; }
        [Inject] private R_IMenuService _menuService { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] private IClientHelper _clientHelper { get; set; }
        [Inject] private R_MessageBoxService _messageBoxService { get; set; }
        [Inject] private R_PreloadService _preloadService { get; set; }
        [Inject] private R_ToastService _toastService { get; set; }

        private List<MenuListDTO> _menuList = new();
        private List<DrawerMenuItem> _data = new();
        private int _maxNotificationCount = 5;
        private List<BlazorMenuNotificationDTO> _newNotificationMessages = new List<BlazorMenuNotificationDTO>();
        private List<BlazorMenuNotificationDTO> _oldNotificationMessages = new List<BlazorMenuNotificationDTO>();

        private string _searchText = string.Empty;
        private string _userId = string.Empty;
        private string _footerId = "navbar-footer";

        private MenuTabSet _menuTabSetRef;
        private MenuOverlay _menuOverlay;
        private string _logoUrl = string.Empty;
        private string _logoStyle = string.Empty;

        private List<SearchBoxItem> searchBoxData
        {
            get
            {
                var loData = _menuList.Where(x => x.CSUB_MENU_TYPE == "P")
                    .GroupBy(x => x.CSUB_MENU_ID)
                    .Select(x => x.First())
                    .Select(x => new SearchBoxItem
                    {
                        Id = x.CSUB_MENU_ID,
                        Text = x.CSUB_MENU_ID + " - " + x.CSUB_MENU_NAME
                    }).ToList();

                return loData;
            }
        }

        protected string NotificationCssClass;
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
                    Text = _menuList.FirstOrDefault(x => x.CMENU_ID == id)?.CMENU_NAME ?? string.Empty,
                    Level = 0,
                    ProgramButton = _menuList.FirstOrDefault(x => x.CMENU_ID == id)?.CPROGRAM_BUTTON ?? string.Empty,
                    Children = _menuList.Where(x => x.CSUB_MENU_TYPE == "G" && x.CMENU_ID == id).Select(y => new DrawerMenuItem
                    {
                        Id = y.CSUB_MENU_ID,
                        Text = y.CSUB_MENU_NAME,
                        Level = 1,
                        ProgramButton = _menuList.FirstOrDefault(x => x.CSUB_MENU_ID == y.CSUB_MENU_ID)?.CPROGRAM_BUTTON ?? string.Empty,
                        Children = _menuList.Where(z => z.CSUB_MENU_TYPE == "P" && z.CPARENT_SUB_MENU_ID == y.CSUB_MENU_ID && z.CMENU_ID == id).Select(yy => new DrawerMenuItem
                        {
                            Id = yy.CSUB_MENU_ID,
                            Text = yy.CSUB_MENU_NAME,
                            Level = 2,
                            ProgramButton = _menuList.FirstOrDefault(x => x.CSUB_MENU_ID == yy.CSUB_MENU_ID)?.CPROGRAM_BUTTON ?? string.Empty,
                            Children = new()
                        }).ToList()
                    }).ToList()
                }).ToList();

                _userId = "TR";

                _logoUrl = "assets/img/logo-bimasakti.png";
                _logoStyle = $"width: 125px; height: 35px; background-image: url({_logoUrl}); background-size: cover; background-position: left center; background-repeat: no-repeat;";

                if (_menuOverlay != null)
                    _menuOverlay.AssignOnClick(OnClickProgram);
            }
            catch (Exception ex)
            {
                await _messageBoxService.Show(ex.Message);
            }
            finally
            {
                await _preloadService.Hide();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync(JsConstants.HandleNavbarVerticalCollapsed);

                DotNetReference = DotNetObjectReference.Create(this);
                await JSRuntime.InvokeVoidAsync(JsConstants.ObserveElement, "navbarDropdownNotification", DotNetReference);

                await JSRuntime.InvokeVoidAsync(JsConstants.ChangeThemeToggle, "themeControlToggle");

                await JSRuntime.InvokeVoidAsync(JsConstants.OverrideDefaultKey, DotNetReference);

                await JSRuntime.InvokeVoidAsync(JsConstants.AttachFocusHandler, DotNetReference, _autoCompleteId);

                await JSRuntime.InvokeVoidAsync(JsConstants.ToggleFooter, _footerId);

                if (_menuOverlay != null)
                    _menuOverlay.AssignOnClick(OnClickProgram);

                // OpenComponent();
            }
        }

        [JSInvokable("DefaultKeyDown")]
        public async Task DefaultKeyDown(KeyboardEventArgs args)
        {
            //var documentationUrl = GetDocumentationBaseUrl();
            //if (string.IsNullOrWhiteSpace(documentationUrl))
            //    return;

            //var url = documentationUrl + ParseProgramId();
            //var currentUrl = new Uri(url);

            //await JSRuntime.InvokeVoidAsync(JsConstants.BlazorOpen, new object[2] { currentUrl, "_blank" });
        }

        [JSInvokable("FindKeyDown")]
        public async Task FindKeyDown(KeyboardEventArgs args)
        {
            if (TelerikAutoCompleteRef is not null)
                await TelerikAutoCompleteRef.FocusAsync();
        }

        [JSInvokable("OpenComponent")]
        public void OpenComponent()
        {
            if (TelerikAutoCompleteRef is not null && !string.IsNullOrWhiteSpace(_searchText))
                TelerikAutoCompleteRef.Open();
        }

        [JSInvokable("ObserverNotification")]
        public async Task ObserverNotification(bool plShow)
        {
            //if (!plShow)
            //{
            //    foreach (var message in _newNotificationMessages)
            //    {
            //        message.IsRead = true;
            //    }

            //    _oldNotificationMessages.AddRange(_newNotificationMessages);
            //    _newNotificationMessages.Clear();
            //}

            await InvokeAsync(StateHasChanged);
        }

        private async Task OnClickShowMenuOverlay(DrawerMenuItem poMenu, string[]? poBreadCrumbs = null)
        {
            if (_menuOverlay != null)
                await _menuOverlay.Show(poMenu, poBreadCrumbs);
        }

        private async Task OnClickProgram(DrawerMenuItem drawerMenuItem)
        {
            await OnClickProgram(drawerMenuItem.Text, drawerMenuItem.Id);
        }

        private async Task OnClickProgram(string text, string id)
        {
            try
            {
                if (_menuOverlay != null)
                    await _menuOverlay.Hide();

                if (_menuTabSetRef is not null)
                {
                    var laMenuList = _menuList.Where(x => x.CSUB_MENU_ID == id).ToList();
                    var laMenuAccess = laMenuList
                                                .SelectMany(x => x.CSUB_MENU_ACCESS?.Split(',') ?? Array.Empty<string>())
                                                .Distinct()
                                                .ToArray();
                    var lcMenuAccess = string.Join(",", laMenuAccess);

                    await _menuTabSetRef.OpenTabAsync(text, id, lcMenuAccess);
                }
            }
            catch (Exception ex)
            {
                await _messageBoxService.Show(ex.Message);
                await _preloadService.Hide();
            }
        }

        private void SearchTextValueChanged(string value)
        {
            _searchText = value;
        }

        private bool searchTextChanged = false;
        private async Task SearchTextOnChange(object value)
        {
            if (string.IsNullOrWhiteSpace(_searchText))
                return;

            var programId = _searchText.Split(" - ")[0];
            var menuItem = _menuList.FirstOrDefault(x => x.CSUB_MENU_ID == programId);

            if (menuItem != null)
            {
                if (searchTextChanged)
                    return;

                searchTextChanged = true;

                if (TelerikAutoCompleteRef is not null)
                    TelerikAutoCompleteRef.Close();

                await OnClickProgram(menuItem.CSUB_MENU_NAME, menuItem.CSUB_MENU_ID);
                _searchText = default;
            }

            searchTextChanged = false;
        }

        private async Task Logout()
        {
            if (_menuOverlay is not null)
                await _menuOverlay.Hide();

            await _preloadService.Show();

            await ((BlazorMenuAuthenticationStateProvider)_stateProvider).MarkUserAsLoggedOut();

            _navigationManager.NavigateTo("/");

            await _preloadService.Hide();
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

    public class BlazorMenuNotificationDTO
    {
        public string Author { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime MessageDate { get; set; }
        public bool IsJustArrived { get; set; } = true;
        public bool IsRead { get; set; }

        public string HtmlMessage => $"<strong>{Author} : </strong> {Message}";
    }
}
