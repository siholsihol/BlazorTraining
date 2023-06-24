using Microsoft.AspNetCore.Components;

namespace BlazorMenu.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        private bool _iconMenuActive { get; set; }
        private string IconMenuCssClass => _iconMenuActive ? "width: 80px;" : null;

        protected void ToggleIconMenu(bool iconMenuActive)
        {
            _iconMenuActive = iconMenuActive;
        }
    }
}
