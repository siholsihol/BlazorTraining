using BlazorMenu.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.Attributes;
using R_BlazorFrontEnd.Controls.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Extensions;
using R_BlazorFrontEnd.Helpers;
using R_BlazorFrontEnd.Interfaces;
using System.Reflection;

namespace BlazorMenu.Shared.Tabs
{
    public class MenuTabsRouteView : RouteView
    {
        [Inject] private MenuTabSetTool TabSetTool { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        //[Inject] private IServiceProvider ServiceProvider { get; set; } = default!;

        private R_ITenant? _tenant;

        protected override void Render(RenderTreeBuilder builder)
        {
            //_tenant = ServiceProvider.GetTenantFromService();

            var leAccess = GetFullFormAccess();

            var url = "/" + NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            if (!url.Equals("/"))
            {
                var selTab = TabSetTool.Tabs.FirstOrDefault(m => !m.IsInited);

                if (selTab != null)
                {
                    leAccess = ConvertStringToFormAccess(selTab.Access.Split(","));
                }
            }

            var body = CreatePage(RouteData, leAccess);

            RenderContentInDefaultLayout(builder, body, true);
        }

        private void RenderContentInDefaultLayout(RenderTreeBuilder builder, RenderFragment body, bool isLoad = false)
        {
            var layoutType = RouteData.PageType.GetCustomAttribute<LayoutAttribute>()?.LayoutType ?? DefaultLayout;
            builder.OpenComponent<CascadingValue<MenuTabsRouteView>>(0);
            builder.AddAttribute(1, "Name", "RouteView");
            builder.AddAttribute(2, "Value", this);
            builder.AddAttribute(3, "ChildContent", (RenderFragment)(b =>
            {
                b.OpenComponent(20, layoutType);
                b.AddAttribute(21, "Body", body);
                b.CloseComponent();
            }));
            builder.CloseComponent();

            var url = "/" + NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            var tenantId = string.Empty;
            if (_tenant is not null)
                tenantId = _tenant.Identifier;

            if (!url.Equals("/" + tenantId))
            {
                var selTab = TabSetTool.Tabs.FirstOrDefault(m => !m.IsInited);

                if (selTab == null)
                {
                    if (TabSetTool.Tabs.Count == 0)
                    {
                        TabSetTool.Tabs.Add(new MenuTab
                        {
                            Body = body,
                            Url = url,
                            IsInited = isLoad,
                            IsActive = true,
                            Title = url.Equals("/") ? "Home" : string.Empty
                        });
                    }
                }
                else
                {
                    selTab.Body = body;
                    selTab.IsActive = true;
                    selTab.PageTitle = GetPageTitle(RouteData.PageType, selTab.AssemblyResourceName);

                    if (isLoad)
                    {
                        selTab.IsInited = true;
                    }
                }
            }
        }

        private RenderFragment CreatePage(RouteData routeData, R_eFormAccess[] peFormAccess)
        {
            RenderFragment page = builder =>
            {
                builder.OpenComponent(0, routeData.PageType);
                foreach (var routeValue in routeData.RouteValues)
                {
                    builder.AddAttribute(1, routeValue.Key, routeValue.Value);
                }

                if (routeData.PageType.IsSubclassOf(typeof(R_Page)))
                {
                    //var loAccess = GetFullFormAccess();
                    builder.AddAttribute(1, "FormAccess", peFormAccess);
                    builder.AddAttribute(2, "FormModel", R_eFormModel.MainForm);
                }

                builder.CloseComponent();
            };

            return page;
        }

        private string GetPageTitle(Type poPageType, string pcAssemblyResourceName)
        {
            var loEx = new R_Exception();
            var lcRtn = string.Empty;

            try
            {
                if (!poPageType.IsSubclassOf(typeof(R_Page)))
                    throw new Exception(R_FrontUtility.R_GetMessage(typeof(BlazorMenuLocalizer), "Page_E001", pcResourceName: "BlazorMenuResources"));

                var loAttributes = poPageType.GetCustomAttributes(true);

                if (loAttributes.FirstOrDefault(x => x is R_PageAttribute) is R_PageAttribute loPageAttribute && loPageAttribute != null)
                {
                    lcRtn = loPageAttribute.Title;

                    if (!string.IsNullOrWhiteSpace(loPageAttribute.ResourceId))
                    {
                        lcRtn = R_FrontUtility.R_GetMessage(pcAssemblyResourceName, loPageAttribute.ResourceId);
                    }
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return lcRtn;
        }

        private static R_eFormAccess[] GetFullFormAccess()
        {
            return new R_eFormAccess[]
            {
                R_eFormAccess.Add,
                R_eFormAccess.Update,
                R_eFormAccess.Delete,
                R_eFormAccess.Print,
                R_eFormAccess.View
            };
        }

        private static R_eFormAccess[] ConvertStringToFormAccess(string[] pcFormAccess)
        {
            var loFormAccess = pcFormAccess.Select(x => x.ToEnum<R_eFormAccess>()).ToArray();

            return loFormAccess;
        }
    }
}
