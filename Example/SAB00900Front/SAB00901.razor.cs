using DataDummyProvider.DTOs;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.Tab;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;

namespace SAB00900Front
{
    public partial class SAB00901 : R_Page, R_ITabPage
    {
        private SAB00900ViewModel ViewModel = new();
        private R_Conductor _conductorRef;

        private R_NumericTextBox<int> _numericIdRef;
        private R_TextBox _productNameRef;
        private R_DropDownList<CategoryDTO, int> _dropdownCategoryRef;
        private R_DatePicker<DateTime?> _datePickerRef;
        private R_CheckBox _checkboxActiveRef;

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                if (poParameter == null)
                    await this.CloseProgramAsync();

                ViewModel.GetCategories();

                if (poParameter is ProductDTO product)
                {
                    await _conductorRef.R_SetCurrentData(product);
                    await _conductorRef.Edit();
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }


        #region Close
        private async Task OnClose()
        {
            var loData = (ProductDTO)_conductorRef.R_GetCurrentData();
            loData.Name = "Dari Popup";
            await this.Close(true, loData);
        }

        private async Task OnCancel()
        {
            await this.Close(false, null);
        }
        #endregion

        #region Assign Result
        protected override Task<object?> R_Set_Result_Detail()
        {
            var loData = (ProductDTO)_conductorRef.R_GetCurrentData();
            loData.Name = "Dari Detail";
            return Task.FromResult<object?>(loData);
        }
        protected override Task<object?> R_Set_Result_PredefinedDock()
        {
            var loData = (ProductDTO)_conductorRef.R_GetCurrentData();
            loData.Name = $"Dari Predefined";
            return Task.FromResult<object?>(loData);
        }
        protected override Task<object?> R_Set_Result_TabPage()
        {
            var loData = (ProductDTO)_conductorRef.R_GetCurrentData();
            loData.Name = "Dari Tab Page";
            return Task.FromResult<object?>(loData);
        }
        #endregion

        #region Predefined Dock
        private void R_InstantiateDock(R_InstantiateDockEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(SAB00901);
        }

        private async Task R_AfterOpenPredefinedDock(R_AfterOpenPredefinedDockEventArgs eventArgs)
        {
            if (eventArgs.Result is null) return;
            await _conductorRef.R_SetCurrentData(eventArgs.Result);
        }
        #endregion

        private void Conductor_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = (ProductDTO)eventArgs.Data;
                ViewModel.GetProductById(loParam.Id);

                eventArgs.Result = ViewModel.Product;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        private async Task Conductor_Display(R_DisplayEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loData = eventArgs.Data as ProductDTO;
                if (loData != null && eventArgs.ConductorMode == R_eConductorMode.Normal)
                {
                    ViewModel.ReleaseDate = loData.ReleaseDate;
                }

                if (loData.Active)
                {
                    ViewModel.TextBox1 = "Aktif";
                }
                else
                {
                    ViewModel.TextBox1 = "Tidak Aktif";
                }

                if (eventArgs.ConductorMode == R_eConductorMode.Add)
                {
                    await _numericIdRef.FocusAsync();
                }
                else if (eventArgs.ConductorMode == R_eConductorMode.Edit)
                {
                    await _productNameRef.FocusAsync();
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

        }

        #region Input Event
        private void DatePicker_ValueChanged(DateTime? pdValue)
        {
            if (pdValue.HasValue)
            {
                ViewModel.ReleaseDate = pdValue.Value;
            }
        }
        public Task RefreshTabPageAsync(object poParam)
        {
            return Task.CompletedTask;
        }
        #endregion
    }
}
