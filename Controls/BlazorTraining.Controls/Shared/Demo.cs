using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using Telerik.Blazor.Components;

namespace BlazorTraining.Controls
{
    public class Demo : ComponentBase
    {
        #region Members

        private string? code;
        private int _selectedTabIndex;
        #endregion

        #region Methods

        protected override async Task OnParametersSetAsync()
        {
            if (code is null && Type is not null)
            {
                var resourceName = Type.FullName + ".razor";
                using (Stream? stream = Type.Assembly.GetManifestResourceStream(resourceName))
                {
                    try
                    {
                        if (stream is null)
                            return;

                        using (StreamReader reader = new StreamReader(stream))
                        {
                            code = await reader.ReadToEndAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            // no base call
            builder.AddMarkupContent(0, "<!--googleoff: index-->");
            builder.OpenElement(1, "div");
            builder.AddAttribute(2, "class", "demo");

            if (ShowCodeOnly)
            {
                builder.OpenElement(10, "div");
                builder.AddAttribute(11, "class", "highlight show-code-only");
                builder.OpenElement(12, "pre");
                builder.OpenElement(13, "code");
                builder.AddAttribute(14, "class", LanguageCssClass);

                if (code != null)
                {
                    builder.AddContent(15, code.Trim());
                }

                builder.CloseElement(); // code
                builder.CloseElement(); // pre
                builder.CloseElement(); // div
            }
            else if (!Tabs)
            {
                builder.OpenElement(20, "div");
                builder.AddAttribute(21, "class", "bb-example");
                builder.OpenComponent(22, Type!);
                builder.CloseComponent();
                builder.CloseElement();

                builder.OpenElement(30, "div");
                builder.AddAttribute(31, "class", "highlight no-tabs");
                builder.OpenElement(32, "pre");
                builder.OpenElement(33, "code");
                builder.AddAttribute(34, "class", LanguageCssClass);

                if (code != null)
                {
                    builder.AddContent(35, code.Trim());
                }

                builder.CloseElement(); // code
                builder.CloseElement(); // pre
                builder.CloseElement(); // div
            }
            else // Tabs = true
            {
                builder.OpenComponent<TelerikTabStrip>(40);
                builder.AddAttribute(41, "PersistTabContent", true);
                builder.AddAttribute(42, "ActiveTabIndex", _selectedTabIndex); builder.AddAttribute(43, "ActiveTabIndexChanged", EventCallback.Factory.Create<int>(this, async index =>
                {
                    _selectedTabIndex = index;
                    // "View Source" tab index
                    if (index == 1)
                    {
                        await JS.InvokeVoidAsync("highlightCode");
                    }
                }));
                builder.AddAttribute(42, "ChildContent", (RenderFragment)((childBuilder) =>
                {
                    childBuilder.OpenComponent<TabStripTab>(50);
                    childBuilder.AddAttribute(51, "Title", "👀 Example");
                    childBuilder.AddAttribute(52, "Content", (RenderFragment)((tabBuilder) =>
                    {
                        tabBuilder.OpenElement(60, "div");
                        tabBuilder.AddAttribute(61, "class", "bb-example");
                        tabBuilder.OpenComponent(62, Type!);
                        tabBuilder.CloseComponent();
                        tabBuilder.CloseElement();
                    }));
                    childBuilder.CloseComponent();

                    childBuilder.OpenComponent<TabStripTab>(70);
                    childBuilder.AddAttribute(71, "Title", "</> View Source");
                    childBuilder.AddAttribute(72, "Content", (RenderFragment)((tabBuilder) =>
                    {
                        tabBuilder.OpenElement(80, "div");
                        tabBuilder.AddAttribute(81, "class", "highlight show-code-only");
                        tabBuilder.OpenElement(82, "pre");
                        tabBuilder.OpenElement(83, "code");
                        tabBuilder.AddAttribute(84, "class", LanguageCssClass);

                        if (code != null)
                        {
                            tabBuilder.AddContent(85, code.Trim());
                        }

                        tabBuilder.CloseElement(); // code
                        tabBuilder.CloseElement(); // pre
                        tabBuilder.CloseElement(); // div
                    }));
                    childBuilder.CloseComponent();
                }));

                builder.CloseComponent();
            }

            builder.CloseElement();
            builder.AddMarkupContent(700, "<!--googleon: index-->");
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            await JS.InvokeVoidAsync("highlightCode");
        }

        #endregion

        #region Properties

        [Inject] protected IJSRuntime JS { get; set; } = null!;

        [Parameter] public string LanguageCssClass { get; set; } = "language-cshtml";

        [Parameter] public bool ShowCodeOnly { get; set; }

        [Parameter] public bool Tabs { get; set; } = false;

        [Parameter] public Type? Type { get; set; }

        #endregion
    }
}
