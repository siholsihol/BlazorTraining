using BlazorMenu.Constants;
using BlazorMenu.Resources;
using BlazorMenu.Shared.Drawer;
using BlazorMenuCommon.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.Base;
using R_BlazorFrontEnd.Controls.Constants;
using R_BlazorFrontEnd.Controls.Helpers;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;

namespace BlazorMenu.Shared.Overlay
{
    public partial class MenuOverlay : BaseComponent, IDisposable
    {
        #region Dependency Injection
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] private R_ToastService ToastService { get; set; } = default!;
        #endregion

        #region Properties
        public override string Id { get; set; } = IdGeneratorHelper.Generate("MenuOverlay");

        protected string ClassNames => new CssBuilder()
            .AddClass("r-menu-overlay")
            //.AddClass("fade")
            //.AddClass("show")
            .Build();

        protected string StyleNames => new StyleBuilder()
            .AddStyle("display:block")
            .Build();
        #endregion

        #region Private Members
        private DrawerMenuItem? _drawerMenuItem;
        private bool _showMenuOverlay;
        private string? _breadCrumbs;
        private string _defaultIconId = AppConstants.MenuIconId;
        private bool _isFavorite;
        private bool _isRendered = true;

        private string _tilesRenderKey = IdGeneratorHelper.Generate("tiles");
        private readonly string _swapyId = "menu-swapy-container";
        private bool _dragEnabled;
        #endregion

        #region Event Callbacks
        [Parameter] public EventCallback<DrawerMenuItem> OnClickProgram { get; set; }
        [Parameter] public EventCallback<string> OnFavorite { get; set; }
        [Parameter] public EventCallback<string> OnUnfavorite { get; set; }
        [Parameter] public EventCallback<SetUserProgramSequenceParameterDTO> OnUpdateSequence { get; set; }
        #endregion

        #region Overlay Show/Hide
        internal async Task Show(DrawerMenuItem menu, string[]? breadCrumbs = null)
        {
            if (_dragEnabled) return;

            _tilesRenderKey = IdGeneratorHelper.Generate("tiles");
            _isFavorite = false;
            _dragEnabled = false;

            if (!_showMenuOverlay)
            {
                await JSRuntime.InvokeVoidAsync(JsConstants.ToggleMenuOverlay, true);
                _showMenuOverlay = true;
            }

            if (breadCrumbs is not null)
                _breadCrumbs = string.Join(" > ", breadCrumbs);

            _drawerMenuItem = menu;
            _isFavorite = _drawerMenuItem.Id == AppConstants.FavoriteMenuId;
            _defaultIconId = $"{AppConstants.MenuIconId}-{_drawerMenuItem.Text.ToLowerInvariant()}";

            StateHasChanged();
            await Task.Delay(1);

            if (_drawerMenuItem.Children.Count > 0 && ElementRef.Context != null)
                await ElementRef.FocusAsync();

            await Task.Delay(1);
        }
        internal async Task Hide()
        {
            if (_dragEnabled) return;

            if (_showMenuOverlay)
            {
                await JSRuntime.InvokeVoidAsync(JsConstants.ToggleMenuOverlay, false);
                _showMenuOverlay = false;
            }

            StateHasChanged();
            await Task.Delay(1);
        }
        #endregion

        #region Drag & Drop
        private async Task ToggleDragDrop(bool enable)
        {
            _dragEnabled = enable;

            if (_dragEnabled)
            {
                await JS.InvokeVoidAsync(JsConstants.InitSwapy, _swapyId);
            }
            else
            {
                if (_drawerMenuItem != null)
                {
                    var parameter = new SetUserProgramSequenceParameterDTO
                    {
                        CMENU_ID = _drawerMenuItem.MenuId,
                        DATA = await GetDragDropData()
                    };

                    await OnUpdateSequence.InvokeAsync(parameter);
                }

                await JS.InvokeVoidAsync(JsConstants.DisposeSwapy, _swapyId);
            }
        }
        private async Task<List<DragDropDataDTO>> GetDragDropData()
        {
            var data = await JS.InvokeAsync<DragDropDTO[]>(JsConstants.GetSwapyData, _swapyId);

            return data.Select((x, index) => new DragDropDataDTO
            {
                ISEQ_NO = index + 1,
                CPROGRAM_ID = x.item.Replace("item-", "")
            }).ToList();
        }
        #endregion

        #region Event Handlers
        private async Task OnClickProgramHandler(DrawerMenuItem program)
        {
            if (_dragEnabled) return;
            await OnClickProgram.InvokeAsync(program);
            await Hide();
        }
        private async Task OnKeyDownHandler(KeyboardEventArgs e)
        {
            if (e.Key == "Escape")
                await Hide();
        }
        private async Task OnKeyDownItemHandler(KeyboardEventArgs e, DrawerMenuItem program)
        {
            if (e.Key == "Enter")
                await OnClickProgramHandler(program);
        }
        private async Task OnClickFavoriteIcon(DrawerMenuItem program)
        {
            var loEx = new R_Exception();
            var oldFav = program.Favorite;
            program.Favorite = !program.Favorite;

            program.IsAnimating = true;
            StateHasChanged();
            await Task.Delay(1);

            _ = Task.Run(async () =>
            {
                await Task.Delay(300);
                program.IsAnimating = false;
                StateHasChanged();
            });

            try
            {
                await Task.Delay(1);

                if (oldFav)
                    await OnUnfavorite.InvokeAsync(program.Id);
                else
                    await OnFavorite.InvokeAsync(program.Id);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            finally
            {
                if (loEx.HasError)
                {
                    ToastService.Error(string.Format(
                        R_FrontUtility.R_GetMessage(typeof(BlazorMenuLocalizer),
                        oldFav ? "Fav_E002" : "Fav_E001",
                        pcResourceName: "BlazorMenuResources"), program.Id));

                    program.Favorite = oldFav;
                }
                else
                {
                    ToastService.Success(string.Format(
                        R_FrontUtility.R_GetMessage(typeof(BlazorMenuLocalizer),
                        oldFav ? "Fav_E004" : "Fav_E003",
                        pcResourceName: "BlazorMenuResources"), program.Id));
                }

                StateHasChanged();
                await Task.Delay(1);
            }

            loEx.ThrowExceptionIfErrors();
        }
        #endregion
    }
}
