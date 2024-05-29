using DataDummyProvider.DTOs;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.Forms;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_BlazorFrontEnd.Controls.Popup;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;

namespace SAB00900Front
{
    public partial class SAB00900 : R_Page
    {
        private SAB00900ViewModel ViewModel = new();
        private R_Conductor _conductorRef;

        protected override Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                ViewModel.GetCategories();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);

            return Task.CompletedTask;
        }

        public void Conductor_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
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

        private R_NumericTextBox<int> _numericIdRef;
        private R_DropDownList<CategoryDTO, int> _dropdownCategoryRef;
        private R_DatePicker<DateTime?> _datePickerRef;
        private R_CheckBox _checkboxActiveRef;
        private async Task Conductor_AfterAdd(R_AfterAddEventArgs eventArgs)
        {
            var loEntity = (ProductDTO)eventArgs.Data;

            loEntity.CategoryId = 1;
            loEntity.ReleaseDate = DateTime.Now;

            await _checkboxActiveRef.FocusAsync();
        }

        public void Conductor_Validation(R_ValidationEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loData = (ProductDTO)eventArgs.Data;

                if (string.IsNullOrWhiteSpace(loData.Name))
                {
                    //loEx.Add("", "Please fill Product Name.");
                    _errorMessage = "Please fill Product Name.";
                }
                else
                {
                    _errorMessage = "";
                }

                if (loData.Price == 0)
                {
                    loEx.Add("", "Please fill Price.");
                }

                //StateHasChanged();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            if (loEx.HasError)
                eventArgs.Cancel = true;

            loEx.ThrowExceptionIfErrors();
        }

        public void Conductor_ServiceSave(R_ServiceSaveEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = (ProductDTO)eventArgs.Data;
                ViewModel.SaveProduct(loParam, eventArgs.ConductorMode);

                eventArgs.Result = ViewModel.Product;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public void Conductor_AfterSave(R_AfterSaveEventArgs eventArgs)
        {
            var loData = (ProductDTO)eventArgs.Data;

            if (eventArgs.ConductorMode == R_eConductorMode.Add)
                R_MessageBox.Show("", $"Add {loData.Id} success.", R_eMessageBoxButtonType.OK);
            else
                R_MessageBox.Show("", $"Edit {loData.Id} success.", R_eMessageBoxButtonType.OK);
        }

        public void Conductor_ServiceDelete(R_ServiceDeleteEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = (ProductDTO)eventArgs.Data;
                ViewModel.DeleteProduct(loParam.Id);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        #region Find
        public void R_Before_Open_Find(R_BeforeOpenFindEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(ProductPage);
            eventArgs.Parameter = "Dari Find";
        }

        public async Task R_After_Open_Find(R_AfterOpenFindEventArgs eventArgs)
        {
            var loData = (ProductDTO)eventArgs.Result;
            var loParam = new ProductDTO { Id = loData.Id };

            await _conductorRef.R_GetEntity(loParam);
        }
        #endregion

        #region Lookup
        public void R_Before_Open_Lookup(R_BeforeOpenLookupEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(ProductPage);
            eventArgs.Parameter = "Dari Lookup";
        }

        public async Task R_After_Open_Lookup(R_AfterOpenLookupEventArgs eventArgs)
        {
            if (eventArgs.Result is null) return;

            var loData = (ProductDTO)eventArgs.Result;
            var loParam = new ProductDTO { Id = loData.Id };

            await _conductorRef.R_GetEntity(loParam);
        }

        [Inject] public R_LookupService LookupService { get; set; }
        private async Task lookupButtonOnClick()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await LookupService.Show(typeof(ProductPage), "Dari LookupService");
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        #endregion

        #region Popup
        public void R_Before_Open_Popup(R_BeforeOpenPopupEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(ProductPage);
            eventArgs.Parameter = "Dari Popup";
        }

        public async Task R_After_Open_Popup(R_AfterOpenPopupEventArgs eventArgs)
        {
            if (!eventArgs.Success || eventArgs.Result is null) return;

            var loData = (ProductDTO)eventArgs.Result;
            var loParam = new ProductDTO { Id = loData.Id };

            await _conductorRef.R_GetEntity(loParam);
        }

        [Inject] public R_PopupService PopupService { get; set; }
        private async Task popupButtonOnClick()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await PopupService.Show(typeof(ProductPage), "Dari PopupService");
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        #endregion

        public async Task Conductor_BeforeEdit(R_BeforeEditEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await PopupService.Show(typeof(ProductPage), "Dari PopupService");

                eventArgs.Cancel = !loResult.Success;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task buttonCloseOnClick()
        {
            await this.CloseProgram();
        }

        //protected override Task R_PageClosing(R_PageClosingEventArgs eventArgs)
        //{
        //    eventArgs.Cancel = true;

        //    return Task.CompletedTask;
        //}

        private string _errorMessage = "";

        public void Conductor_BeforeCancel()
        {
            _errorMessage = "";
        }
    }
}
