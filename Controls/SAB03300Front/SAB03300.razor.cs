using DataProvider.DTOs;
using DataProvider.Services;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Exceptions;

namespace SAB03300Front
{
    public partial class SAB03300
    {
        private R_ConductorGrid _conGridProductRef;
        private R_Grid<ProductDTO> _gridRef;

        private SAB03300ViewModel _viewModel = new();

        [Inject] private IProductService ProductService { get; set; }
        [Inject] private ICategoryService CategoryService { get; set; }

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                _viewModel = new SAB03300ViewModel(ProductService, CategoryService);

                await _viewModel.GetCategoriesAsync();
                await _gridRef.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                await _viewModel.GetProductsAsync();

                eventArgs.ListEntityResult = _viewModel.Products;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void R_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            eventArgs.Result = eventArgs.Data;
        }

        private void R_AfterAdd(R_AfterAddEventArgs eventArgs)
        {
            //var loData = (ProductDTO)eventArgs.Data;

            //loData.ReleaseDate = DateTime.Now;
        }

        #region Save Batch

        private void R_BeforeSaveBatch(R_BeforeSaveBatchEventArgs events)
        {
            var loData = (List<ProductDTO>)events.Data;

            events.Cancel = loData.Count == 0;
        }

        private void R_ServiceSaveBatch(R_ServiceSaveBatchEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void R_AfterSaveBatch(R_AfterSaveBatchEventArgs eventArgs)
        {

        }

        #endregion

        private async Task OnClickSave()
        {
            await _gridRef.R_SaveBatch();
        }

        #region Drag Drop

        private void R_GridRowBeforeDrop(R_GridDragDropBeforeDropEventArgs<ProductDTO> eventArgs)
        {
            //eventArgs.Cancel = true;
        }

        private void R_GridRowAfterDrop(R_GridDragDropAfterDropEventArgs<ProductDTO> eventArgs)
        {

        }

        private async Task OnClickNext()
        {
            await _gridRef.R_MoveToNextRow();
        }

        private async Task OnClickPrevious()
        {
            await _gridRef.R_MoveToPreviousRow();
        }

        private async Task OnClickFirst()
        {
            await _gridRef.R_MoveToFirstRow();
        }

        private async Task OnClickLast()
        {
            await _gridRef.R_MoveToLastRow();
        }

        #endregion
    }
}
