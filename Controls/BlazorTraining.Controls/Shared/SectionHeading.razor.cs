using BlazorTraining.Controls.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorTraining.Controls
{
    public partial class SectionHeading : ComponentBase
    {
        //#region Members

        //private string link => $"{PageUrl}#{HashTagName}".Trim().ToLower();

        //#endregion

        #region Properties

        [Parameter] public HeadingSize Size { get; set; }

        [Parameter] public string Text { get; set; } = null!;

        //[Parameter] public string PageUrl { get; set; } = null!;

        [Parameter] public string HashTagName { get; set; } = null!;

        [Inject] protected IJSRuntime JS { get; set; } = null!;

        #endregion

        #region Methods

        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    await Task.Delay(200);
        //    await JS.InvokeVoidAsync("navigateToHeading", HashTagName);
        //    await base.OnAfterRenderAsync(firstRender);
        //}

        private async Task OnClick()
        {
            await JS.InvokeVoidAsync("navigateToHeading", HashTagName);
        }

        #endregion
    }
}
