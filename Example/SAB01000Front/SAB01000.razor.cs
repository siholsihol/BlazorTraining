using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Exceptions;
using SAB01000Front.DTOs;

namespace SAB01000Front
{
    public partial class SAB01000 : R_Page
    {
        private R_ConductorGrid _conGridProductRef;
        private R_Grid<SelectedProductDTO> _gridRef;

        private SAB01000ViewModel _viewModel = new();

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                _viewModel.GenerateCategory();
                await _gridRef.R_RefreshGrid(null);
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
                _viewModel.GenerateProduct(10);

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
            var loData = (SelectedProductDTO)eventArgs.Data;

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
    }
}
