using DataDummyProvider.DTOs;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Enums;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.Grid;
using R_BlazorFrontEnd.Controls.Grid.Columns.ColumnInfo;
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
            var loData = (List<SelectedProductDTO>)events.Data;

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

        private string _headerTextPrice = "Price";
        private Task OnClickSave()
        {
            //await _conGridProductRef.R_SaveBatch();
            //await _gridRef.R_SaveBatch();

            foreach (var a in _viewModel.Products)
            {
                if (a.Selected == true)
                    a.Price = 0;
            }

            _headerTextPrice = "test price";

            return Task.CompletedTask;
        }

        private void R_CellValueChanged(R_CellValueChangedEventArgs eventArgs)
        {
            var loMultiHeaderColumn = eventArgs.Columns.Where(x => x.GridColumnType == R_eGridColumnType.MultiHeader).FirstOrDefault() as R_GridMultiHeaderColumnInfo;

            if (eventArgs.ColumnName == nameof(SelectedProductDTO.Id) && loMultiHeaderColumn.ChildColumn.Count != 0)
            {
                var loNameColumn = loMultiHeaderColumn.ChildColumn.FirstOrDefault(x => x.Name == "Name");
                loNameColumn.Enabled = false;
            }

            if (eventArgs.ColumnName == nameof(SelectedProductDTO.CategoryId))
            {
                var loPriceColumn = eventArgs.Columns.FirstOrDefault(x => x.Name == "Price");

                var liValue = Convert.ToInt32(eventArgs.Value);

                if (liValue == 2)
                    loPriceColumn.Enabled = false;
                else
                    loPriceColumn.Enabled = true;
            }
        }

        private void R_CheckBoxSelectValueChanged(R_CheckBoxSelectValueChangedEventArgs eventArgs)
        {
            //var loData = (SelectedProductDTO)eventArgs.CurrentRow;

            //eventArgs.Enabled = !IsIdBelow5000(loData);
            eventArgs.Enabled = true;
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

        private async Task OnClickGroup()
        {
            var loGroup = new List<R_GridGroupDescriptor>
            {
                new R_GridGroupDescriptor
                {
                    FieldName = nameof(SelectedProductDTO.CategoryId)
                },

                new R_GridGroupDescriptor
                {
                    FieldName = nameof(SelectedProductDTO.SupplierId)
                }
            };

            await _gridRef.R_GroupBy(loGroup);
        }

        private int _maxLength = 5;
        private void OnClickMaxLength()
        {
            _maxLength = 10;
        }

        private void R_Display(R_DisplayEventArgs eventArgs)
        {

        }

        private DateTime _minDateValue = DateTime.Now;
        private DateTime _maxDateValue = DateTime.Now.AddDays(3);

        private DateTime _changeMinDate = DateTime.Now.AddDays(2);
        private DateTime _changeMaxDate = DateTime.Now.AddDays(4);

        private void OnClickMinDate()
        {
            _minDateValue = _changeMinDate;
        }

        private void OnClickMaxDate()
        {
            _maxDateValue = _changeMaxDate;
        }

        private void R_CheckBoxSelectRender(R_CheckBoxSelectRenderEventArgs eventArgs)
        {
            var loData = (SelectedProductDTO)eventArgs.Data;

            eventArgs.Enabled = !IsIdBelow5000(loData);
        }

        private bool IsIdBelow5000(SelectedProductDTO product)
        {
            return product.Id < 5000;
        }

        private void R_CheckBoxSelectValueChanging(R_CheckBoxSelectValueChangingEventArgs eventArgs)
        {
            //var loData = (SelectedProductDTO)eventArgs.CurrentRow;

            //eventArgs.Cancel = loData.Id > 9000;
        }
    }
}
