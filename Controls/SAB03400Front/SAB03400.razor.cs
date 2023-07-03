using DataDummyProvider.DTOs;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Exceptions;

namespace SAB03400Front
{
    public partial class SAB03400
    {
        private R_ConductorGrid _conGridProductRef;
        private R_ConductorGrid _conGridProductRef2;

        private R_Grid<ProductDTO> _gridRef;
        private R_Grid<ProductDTO> _gridRef2;

        private SAB03400ViewModel _viewModel = new();

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                _viewModel.GetCategories();
                await _gridRef.R_RefreshGrid(null);
                await _gridRef2.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                _viewModel.GetProducts();

                eventArgs.ListEntityResult = _viewModel.Products;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void R_ServiceGetListRecordGrid2(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                eventArgs.ListEntityResult = _viewModel.Products2;
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
            var loData = (ProductDTO)eventArgs.Data;

            loData.ReleaseDate = DateTime.Now;
        }

        #region Save Batch
        private void R_BeforeSaveBatch(R_BeforeSaveBatchEventArgs events)
        {
            //var loData = (List<UserDTO>)events.Data;

            //events.Cancel = loData.Count == 0;
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
            await _conGridProductRef.R_SaveBatch();
        }

        #region Drag Drop
        private void R_GridRowBeforeDrop(R_GridRowBeforeDropEventArgs eventArgs)
        {
            //eventArgs.Cancel = true;
        }

        private void R_GridRowAfterDrop(R_GridRowAfterDropEventArgs eventArgs)
        {

        }

        private async Task OnClickRight()
        {
            await _gridRef.R_MoveToTargetGrid();
        }

        private async Task OnClickMoveAll()
        {
            await _gridRef.R_MoveAllToTargetGrid();
        }

        //private async Task OnClickFirst()
        //{
        //    await _gridRef.R_MoveToFirstRow();
        //}

        //private async Task OnClickLast()
        //{
        //    await _gridRef.R_MoveToLastRow();
        //}
        #endregion
    }
}
