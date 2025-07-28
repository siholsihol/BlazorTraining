using BlazorMenu.Shared.Drawer;

namespace BlazorMenu.Shared.MenuOverlay
{
    public class MenuOverlayService
    {
        internal event Func<List<DrawerMenuItem>, string[]?, Task>? OnShow;
        internal event Func<Task>? OnHide;
        internal event Action<Func<string, string, Task>>? OnAssignOnClick;

        public async Task Show(List<DrawerMenuItem> poMenuList, string[]? poBreadCrumbs = null) => await (OnShow?.Invoke(poMenuList, poBreadCrumbs) ?? Task.CompletedTask);
        public async Task Hide() => await (OnHide?.Invoke() ?? Task.CompletedTask);
        public void AssignOnClick(Func<string, string, Task> poFunction) => OnAssignOnClick?.Invoke(poFunction);
    }
}
