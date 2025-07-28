using BlazorMenu.Constants;
using BlazorMenu.Shared.Drawer;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls.Base;
using R_BlazorFrontEnd.Controls.Helpers;

namespace BlazorMenu.Shared.MenuOverlay
{
    public partial class MenuOverlay : BaseComponent, IDisposable
    {
        #region Properties

        [Inject] private MenuOverlayService MenuOverlayService { get; set; } = default!;

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

        protected override void OnInitialized()
        {
            MenuOverlayService.OnShow += OnShow;
            MenuOverlayService.OnHide += OnHide;
            MenuOverlayService.OnAssignOnClick += OnAssignOnClick;
        }

        private async Task OnClickProgram(string text, string id)
        {
            await OnClickProgramDelegate.Invoke(text, id);
            await OnHide();
        }

        private Func<string, string, Task> OnClickProgramDelegate = async (_, _) => await Task.CompletedTask;

        private void OnAssignOnClick(Func<string, string, Task> poFunction)
        {
            OnClickProgramDelegate = poFunction;
        }

        private async Task OnShow(DrawerMenuItem poMenu, string[]? poBreadCrumbs = null)
        {
            if (poBreadCrumbs is not null)
                _breadCrumbs = string.Join(" > ", poBreadCrumbs);

            _drawerMenuItem = poMenu;

            _showMenuOverlay = true;

            _defaultIconId = AppConstants.MenuIconId + "-" + _drawerMenuItem.Text;

            StateHasChanged();

            await Task.Delay(1);

            await ElementRef.FocusAsync();

            await Task.Delay(1);
        }

        private async Task OnHide()
        {
            _showMenuOverlay = false;

            StateHasChanged();

            await Task.Delay(1);
        }

        protected override async ValueTask DisposeAsync(bool disposing)
        {
            if (disposing)
            {
                MenuOverlayService.OnShow -= OnShow;
                MenuOverlayService.OnHide -= OnHide;
            }

            await base.DisposeAsync(disposing);
        }
        #endregion
    }
}
