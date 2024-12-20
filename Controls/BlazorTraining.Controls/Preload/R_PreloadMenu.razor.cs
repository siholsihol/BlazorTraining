using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls.Base;
using R_BlazorFrontEnd.Controls.Helpers;

namespace BlazorTraining.Controls.Preload
{
    public partial class R_PreloadMenu : BaseComponent, IDisposable
    {
        #region Properties

        [Parameter] public RenderFragment ChildContent { get; set; }
        [Inject] private R_PreloadMenuService PageLoadingService { get; set; }

        public override string Id { get; set; } = IdGeneratorHelper.Generate("preload");

        #endregion

        #region Members

        protected string ClassNames =>
            new CssBuilder()
            .AddClass("modal")
            .AddClass("modal-page-loading")
            .AddClass("fade")
            .AddClass("show", _showBackdrop)
            .Build();

        protected string StyleNames =>
            new StyleBuilder()
            .AddStyle("display:block", _showBackdrop)
            .AddStyle("display:none", !_showBackdrop)
            .Build();

        private bool _showBackdrop;

        #endregion

        #region Methods

        protected override void OnInitialized()
        {
            PageLoadingService.OnShow += OnShow;
            PageLoadingService.OnHide += OnHide;
        }

        private void OnShow()
        {
            _showBackdrop = true;

            StateHasChanged();
        }

        private void OnHide()
        {
            _showBackdrop = false;

            StateHasChanged();
        }

        protected override async ValueTask DisposeAsync(bool disposing)
        {
            if (disposing)
            {
                PageLoadingService.OnShow -= OnShow;
                PageLoadingService.OnHide -= OnHide;
            }

            await base.DisposeAsync(disposing);
        }

        #endregion
    }
}
