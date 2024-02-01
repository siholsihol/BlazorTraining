using DataDummyProvider.DTOs;
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
                _viewModel.GetCategories();
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
                _viewModel.GetProducts();

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

        private void R_CellValueChanged(R_CellValueChangedEventArgs eventArgs)
        {
            //if (eventArgs.ColumnName == "Active")
            //{
            //    var loCategoryIdColumn = eventArgs.Columns.FirstOrDefault(x => x.Name == "CategoryId");
            //    loCategoryIdColumn.Enabled = (bool)eventArgs.Value;
            //}

            if (eventArgs.ColumnName == "CategoryId")
            {
                var loPriceColumn = eventArgs.Columns.FirstOrDefault(x => x.Name == "Price");

                if ((string)eventArgs.Value == "2")
                    loPriceColumn.Enabled = false;
                else
                    loPriceColumn.Enabled = true;
            }

            //if (eventArgs.ColumnName == "ReleaseDate")
            //{
            //    var loPriceColumn = eventArgs.Columns.FirstOrDefault(x => x.Name == "Price");

            //    if (((DateTime)eventArgs.Value).Date == DateTime.Now.Date)
            //        loPriceColumn.Enabled = false;
            //    else
            //        loPriceColumn.Enabled = true;
            //}
        }

        private void R_CellLostFocused(R_CellLostFocusedEventArgs eventArgs)
        {
            if (eventArgs.ColumnName == "Name")
            {
                var loCategoryIdColumn = eventArgs.Columns.FirstOrDefault(x => x.Name == "CategoryId");

                if ((string)eventArgs.Value == "aa")
                    loCategoryIdColumn.Enabled = false;
                else
                    loCategoryIdColumn.Enabled = true;
            }
        }

        #region Lookup
        public void R_Before_Open_Lookup(R_BeforeOpenGridLookupColumnEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(CategoryPage);
            eventArgs.Parameter = _viewModel.Categories;
        }

        public void R_After_Open_Lookup(R_AfterOpenGridLookupColumnEventArgs eventArgs)
        {
            var loResult = eventArgs.Result as CategoryDTO;
            ((SelectedProductDTO)eventArgs.ColumnData).CategoryId = loResult.Id;
        }
        #endregion
    }
}
