using BlazorMenu.Services;
using BlazorMenu.Shared.Drawer;
using BlazorMenu.Shared.Tabs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorMenu.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        private List<DrawerMenuItem> _data = new();
        private bool Expanded = true;
        [Inject] private R_IMenuService _menuService { get; set; }
        [Inject] private MenuTabSetTool TabSetTool { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            var menuList = await _menuService.GetMenuAsync();

            var menuIds = menuList.Where(x => x.CMENU_ID != "FAV")
                .GroupBy(x => x.CMENU_ID)
                .Select(x => x.First()).Select(x => x.CMENU_ID).ToArray();

            _data = menuIds.Select(id => new DrawerMenuItem
            {
                Id = id,
                Text = menuList.FirstOrDefault(x => x.CMENU_ID == id).CMENU_NAME,
                Level = 0,
                Children = menuList.Where(x => x.CSUB_MENU_TYPE == "G" && x.CMENU_ID == id).Select(y => new DrawerMenuItem
                {
                    Id = y.CSUB_MENU_ID,
                    Text = y.CSUB_MENU_NAME,
                    Level = 1,
                    Children = menuList.Where(z => z.CSUB_MENU_TYPE == "P" && z.CPARENT_SUB_MENU_ID == y.CSUB_MENU_ID && z.CMENU_ID == id).Select(yy => new DrawerMenuItem
                    {
                        Id = yy.CSUB_MENU_ID,
                        Text = yy.CSUB_MENU_NAME,
                        Level = 2,
                        Children = new()
                    }).ToList()
                }).ToList()
            }).ToList();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("setFluid");

                await JSRuntime.InvokeVoidAsync("setNavbarStyle");

                await JSRuntime.InvokeVoidAsync("setNavbarPosition");

                await JSRuntime.InvokeVoidAsync("tooltipInit");
            }
        }

        private async Task OnClick()
        {
            Expanded = !Expanded;

            if (!Expanded)
                await JSRuntime.InvokeVoidAsync("setNavbarCollapse");
            else
                await JSRuntime.InvokeVoidAsync("setNavbarShow");
        }

        private void OnClickProgram(DrawerMenuItem drawerMenuItem)
        {
            TabSetTool.AddTab(drawerMenuItem.Text, drawerMenuItem.Id, "A,U,D,P,V");
        }
    }
}
