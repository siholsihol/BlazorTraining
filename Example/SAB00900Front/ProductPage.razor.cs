using DataDummyProvider.DTOs;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_BlazorFrontEnd.Exceptions;

namespace SAB00900Front
{
    public partial class ProductPage : R_Page
    {
        private ProductPageViewModel GridViewModel = new();
        private R_Grid<ProductDTO> _gridRef;
        private string Parameter = "";
        [Inject] public R_MessageBoxService MessageBoxService { get; set; }

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                //var leResult = await MessageBoxService.Show("test", "test", R_eMessageBoxButtonType.OK);

                Parameter = (string)poParameter;
                await _gridRef.R_RefreshGrid(null);

                //loEx.Add(new R_Error("01", "test exception"));
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task OnClose()
        {
            await this.Close(true, _gridRef.GetCurrentData());
        }

        private void R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                GridViewModel.GetProductList();

                eventArgs.ListEntityResult = GridViewModel.ProductList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void R_Before_Open_Popup(R_BeforeOpenPopupEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(ProductPage);
            eventArgs.Parameter = "Dari Popup dalam popup";
        }

        private void R_After_Open_Popup(R_AfterOpenPopupEventArgs eventArgs)
        {

        }

        protected override async Task R_PageClosing(R_PageClosingEventArgs eventArgs)
        {
            R_eMessageBoxResult result = await MessageBoxService.Show("Exit Confirmation",
            "Are you sure to close this page?",
            R_eMessageBoxButtonType.OKCancel);

            eventArgs.Cancel = result != R_eMessageBoxResult.OK;
        }

        private void ThrowException()
        {
            var loEx = new R_Exception();

            loEx.Add(new R_Error("01", "test exception"));

            //loEx.ThrowExceptionIfErrors();
            R_DisplayException(loEx);
        }
    }
}
