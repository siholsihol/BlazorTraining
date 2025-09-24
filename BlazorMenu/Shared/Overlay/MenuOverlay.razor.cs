using BlazorMenu.Constants;
using BlazorMenu.Shared.Drawer;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using R_BlazorFrontEnd.Controls.Base;
using R_BlazorFrontEnd.Controls.Constants;
using R_BlazorFrontEnd.Controls.Helpers;

namespace BlazorMenu.Shared.Overlay
{
    public partial class MenuOverlay : BaseComponent, IDisposable
    {
        #region Properties
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        public override string Id { get; set; } = IdGeneratorHelper.Generate("MenuOverlay");
        #endregion

        #region Members

        protected string ClassNames =>
            new CssBuilder()
            .AddClass("r-menu-overlay")
            //.AddClass("fade")
            //.AddClass("show")
            .Build();

        protected string StyleNames =>
            new StyleBuilder()
            .AddStyle("display:block")
            .Build();

        private DrawerMenuItem? _drawerMenuItem { get; set; }
        private bool _showMenuOverlay;
        private string? _breadCrumbs;
        private string _defaultIconId = AppConstants.MenuIconId;

        #endregion

        #region Methods

        private async Task OnClickProgram(string text, string id)
        {
            await OnClickProgramDelegate.Invoke(text, id);
            await Hide();
        }

        internal Func<string, string, Task> OnClickProgramDelegate = async (_, _) => await Task.CompletedTask;

        internal void AssignOnClick(Func<string, string, Task> poFunction)
        {
            OnClickProgramDelegate = poFunction;
        }

        internal async Task Show(DrawerMenuItem poMenu, string[]? poBreadCrumbs = null)
        {
            if (_showMenuOverlay != true)
            {
                await JSRuntime.InvokeVoidAsync(JsConstants.ToggleMenuOverlay, true);

                _showMenuOverlay = true;
            }

            if (poBreadCrumbs is not null)
                _breadCrumbs = string.Join(" > ", poBreadCrumbs);

            _drawerMenuItem = poMenu;

            _defaultIconId = AppConstants.MenuIconId + "-" + _drawerMenuItem.Text.ToLowerInvariant();

            StateHasChanged();

            await Task.Delay(1);

            if (_drawerMenuItem.Children.Count > 0)
                await ElementRef.FocusAsync();

            await Task.Delay(1);
        }

        internal async Task Hide()
        {
            if (_showMenuOverlay != false)
            {
                await JSRuntime.InvokeVoidAsync(JsConstants.ToggleMenuOverlay, false);

                _showMenuOverlay = false;
            }

            StateHasChanged();

            await Task.Delay(1);
        }

        private async Task HandleKeyDown(KeyboardEventArgs e)
        {
            if (e.Key == "Escape")
            {
                await Hide();
            }
        }

        private async Task HandleKeyDownItem(KeyboardEventArgs e, string text, string id)
        {
            if (e.Key == "Enter")
            {
                await OnClickProgram(text, id);
            }
        }

        #endregion
    }
}
