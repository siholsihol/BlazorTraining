using BlazorClientHelper;
using BlazorMenu.Authentication;
using BlazorMenu.Constants;
using BlazorMenu.Helper;
using BlazorMenu.Managers.Menu;
using BlazorMenu.Pages;
using BlazorMenu.Resources;
using BlazorMenu.Services;
using BlazorMenu.Shared.Drawer;
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
using R_BlazorFrontEnd.Controls.Popup;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using System.Collections.ObjectModel;
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
        [Inject] private IMenuManager MenuManager { get; set; } = default!;
        [Inject] private R_ToastService ToastService { get; set; } = default!;
        [Inject] private R_PopupService PopupService { get; set; } = default!;
        [Inject] private HttpClient Http { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        private List<MenuListDTO> _menuList = new();
        private List<DrawerMenuItem> _data = new();
        private DrawerMenuItem? _favData = null;
        private int _maxNotificationCount = 5;
        private List<BlazorMenuNotificationDTO> _newNotificationMessages = new List<BlazorMenuNotificationDTO>();
        private List<BlazorMenuNotificationDTO> _oldNotificationMessages = new List<BlazorMenuNotificationDTO>();

        private string _searchText = string.Empty;
        private string _userId = string.Empty;
        private string _footerId = "navbar-footer";
        private string? _userIcon = null;

        private MenuTabSet _menuTabSetRef;
        private MenuOverlay _menuOverlay;
        private string _logoUrl = string.Empty;
        private string _logoStyle = string.Empty;

        private ObservableCollection<SearchBoxItem> _searchBoxData = new ObservableCollection<SearchBoxItem>();
        private ObservableCollection<SearchBoxItem> SearchBoxData
        {
            get => _searchBoxData;
        }

        protected string NotificationCssClass;
        private DotNetObjectReference<MenuLayout> DotNetReference { get; set; }

        private TelerikAutoComplete<SearchBoxItem> TelerikAutoCompleteRef;
        private string _autoCompleteId = IdGeneratorHelper.Generate("searchbox", 3);

        protected override async Task OnInitializedAsync()
        {
            await _preloadService.Show();

            try
            {
                await GetMenuListAsync();

                _userId = "TR";

                var baseUri = NavigationManager.BaseUri;
                var iconsByte = await Http.GetByteArrayAsync($"{baseUri}assets/icons/menu-icon.svg");

                if (iconsByte is not null)
                {
                    if (BlazorMenuUtility.GetMenuSVGIds().Length == 0)
                    {
                        await _preloadService.Show();
                        BlazorMenuUtility.SetMenuIconSVGIds(await BlazorMenuUtility.GetSvgSymbolIdsFromFile(iconsByte, JSRuntime));
                        await _preloadService.Hide();
                    }
                }

                _logoUrl = "assets/img/logo-bimasakti.png";
                _logoStyle = $"width: 125px; height: 35px; background-image: url({_logoUrl}); background-size: cover; background-position: left center; background-repeat: no-repeat;";

            }
            catch (Exception ex)
            {
                await _messageBoxService.Show(ex.Message);
            }
            finally
            {
                await _preloadService.Hide();
                if (_favData != null)
                    await OnClickShowMenuOverlay(_favData, new string[] { _favData.Text });
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

        private async Task GetMenuListAsync()
        {
            _menuList = await _menuService.GetMenuAsync();

            var menuGroups = _menuList
                .Where(x => x.CMENU_ID != "FAV")
                .ToLookup(x => x.CMENU_ID);

            var subMenusByParent = _menuList
                .Where(x => x.CSUB_MENU_TYPE == "P")
                .ToLookup(x => x.CMENU_ID + "-" + x.CPARENT_SUB_MENU_ID);

            _favData = menuGroups[AppConstants.FavoriteMenuId]
                .Take(1)
                .Select(menu => new DrawerMenuItem
                {
                    Id = menu.CMENU_ID,
                    Text = menu.CMENU_NAME,
                    Level = 0,
                    MenuId = menu.CMENU_ID,
                    Children = menuGroups[AppConstants.FavoriteMenuId]
                        .Where(p => p.CSUB_MENU_TYPE == "P")
                        .Select(p => new DrawerMenuItem
                        {
                            Id = p.CSUB_MENU_ID,
                            Text = p.CSUB_MENU_NAME,
                            Level = 1,
                            Seq = p.ICOLUMN_INDEX ?? 0,
                            Title = p.CSUB_MENU_TOOL_TIP ?? string.Empty,
                            Children = new(),
                            Favorite = true,
                            MenuId = menu.CMENU_ID,
                        })
                        .OrderBy(p => p.Seq)
                        .ToList()
                }).FirstOrDefault();

            _data = menuGroups
                .Select(group => new DrawerMenuItem
                {
                    Id = group.Key,
                    Text = group.First().CMENU_NAME ?? string.Empty,
                    Level = 0,
                    ProgramButton = group.First().CPROGRAM_BUTTON ?? string.Empty,
                    Children = group
                        .Where(x => x.CSUB_MENU_TYPE == "G")
                        .Select(g => new DrawerMenuItem
                        {
                            Id = g.CSUB_MENU_ID,
                            Text = g.CSUB_MENU_NAME ?? string.Empty,
                            Level = 1,
                            ProgramButton = g.CPROGRAM_BUTTON ?? string.Empty,
                            Children = subMenusByParent[g.CMENU_ID + "-" + g.CSUB_MENU_ID]
                                .Select(p => new DrawerMenuItem
                                {
                                    Id = p.CSUB_MENU_ID,
                                    Text = p.CSUB_MENU_NAME ?? string.Empty,
                                    Level = 2,
                                    Seq = p.ICOLUMN_INDEX ?? 0,
                                    ProgramButton = p.CPROGRAM_BUTTON ?? string.Empty,
                                    Title = p.CSUB_MENU_TOOL_TIP ?? string.Empty,
                                    Favorite = p.LFAVORITE ?? false,
                                    MenuId = group.Key,
                                    Children = new()
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToList();

            _searchBoxData = new ObservableCollection<SearchBoxItem>(
                _menuList
                    .Where(x => x.CSUB_MENU_TYPE == "P")
                    .GroupBy(x => x.CSUB_MENU_ID)
                    .OrderBy(x => x.Key)
                    .Select(x => new SearchBoxItem
                    {
                        Id = x.Key,
                        Text = $"{x.Key} - {x.First().CSUB_MENU_NAME}"
                    })
            );

            await InvokeAsync(StateHasChanged);
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
        private async Task ShowProfilePage()
        {
            var loPopupSettings = new R_PopupSettings()
            {
                PageTitle = @R_FrontUtility.R_GetMessage(typeof(BlazorMenuLocalizer), "_Profile", pcResourceName: "BlazorMenuResources"),
            };

            var loResult = await PopupService.Show(typeof(Profile), new object(), poPopupSettings: loPopupSettings);

            if (loResult.Success)
            {
                ToastService.Success(R_FrontUtility.R_GetMessage(typeof(BlazorMenuLocalizer), "Profile_M001", pcResourceName: "BlazorMenuResources"));
            }
        }
        #endregion

        #region Info Page
        private async Task ShowInfoPage()
        {
            var loPopupSettings = new R_PopupSettings()
            {
                PageTitle = @R_FrontUtility.R_GetMessage(typeof(BlazorMenuLocalizer), "_MoreInformation", pcResourceName: "BlazorMenuResources"),
            };

            var loResult = await PopupService.Show(typeof(Info), new object(), poPopupSettings: loPopupSettings);
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

        #region MenuOverlay
        private async Task SetFavoriteAsync(string pcProgramId)
        {
            var loEx = new R_Exception();

            try
            {
                await MenuManager.SetFavoriteAsync(new FavoriteParameterDTO()
                {
                    CCOMPANY_ID = _clientHelper.CompanyId,
                    CUSER_ID = _clientHelper.UserId,
                    CPROGRAM_ID = pcProgramId
                });

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            finally
            {
                if (!loEx.HasError)
                {
                    await GetMenuListAsync();
                }
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task SetUnfavoriteAsync(string pcProgramId)
        {
            var loEx = new R_Exception();

            try
            {
                await MenuManager.SetUnfavoriteAsync(new FavoriteParameterDTO()
                {
                    CCOMPANY_ID = _clientHelper.CompanyId,
                    CUSER_ID = _clientHelper.UserId,
                    CPROGRAM_ID = pcProgramId
                });
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            finally
            {
                if (!loEx.HasError)
                {
                    await GetMenuListAsync();
                }
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task SetUserProgramSequenceAsync(SetUserProgramSequenceParameterDTO poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                poParameter.CCOMPANY_ID = _clientHelper.CompanyId;
                poParameter.CUSER_ID = _clientHelper.UserId;

                await MenuManager.SetUserProgramSequenceAsync(poParameter);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            finally
            {
                if (!loEx.HasError)
                {
                    await GetMenuListAsync();
                }
            }

            loEx.ThrowExceptionIfErrors();
        }
        #endregion
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
