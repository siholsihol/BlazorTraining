using DataDummyProvider.DTOs;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Enums;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.Forms;
using R_BlazorFrontEnd.Controls.Lookup;
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
        [Inject] public R_IFileConverter _fileConverter { get; set; }

        private bool _isInCRUDMode = false;
        private bool _hasData = false;
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

            loEx.ThrowExceptionIfErrors();
            return Task.CompletedTask;
        }
        protected override Task R_PageClosing(R_PageClosingEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                if (_isInCRUDMode)
                {
                    eventArgs.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();

            return Task.CompletedTask;
        }
        protected override Task<bool> R_LockUnlock(R_LockUnlockEventArgs eventArgs)
        {
            return Task.FromResult(true);
        }

        private R_NumericTextBox<int> _numericIdRef;
        private R_TextBox _productNameRef;
        private R_DropDownList<CategoryDTO, int> _dropdownCategoryRef;
        private R_DatePicker<DateTime?> _datePickerRef;
        private R_CheckBox _checkboxActiveRef;

        #region Conductor Event
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
        #region Check
        private void Conductor_CheckAdd(R_CheckAddEventArgs eventArgs)
        {

        }
        private void Conductor_CheckEdit(R_CheckEditEventArgs eventArgs)
        {
            if (ViewModel.Data.Active)
            {
                eventArgs.Allow = false;
            }
        }
        private void Conductor_CheckDelete(R_CheckDeleteEventArgs eventArgs)
        {
            if (ViewModel.Data.Active)
            {
                eventArgs.Allow = false;
            }
        }
        #endregion

        #region Add
        private async Task Conductor_BeforeAdd(R_BeforeAddEventArgs eventArgs)
        {
            var loResult = await MessageBoxService.Show("Dari Before Add", "Mau Add?", R_eMessageBoxButtonType.YesNo);

            if (loResult == R_eMessageBoxResult.No)
            {
                eventArgs.Cancel = true;
            }
        }
        private void Conductor_AfterAdd(R_AfterAddEventArgs eventArgs)
        {
            var loEntity = (ProductDTO)eventArgs.Data;

            loEntity.CategoryId = 1;
            loEntity.ReleaseDate = DateTime.Now;
        }
        #endregion

        #region Edit
        private async Task Conductor_BeforeEdit(R_BeforeEditEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await MessageBoxService.Show("Dari Before Edit", "Mau Edit?", R_eMessageBoxButtonType.YesNo);

                if (loResult == R_eMessageBoxResult.No)
                {
                    eventArgs.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            var loData = (ProductDTO)eventArgs.Data;
            if (loData.Active)
            {
                eventArgs.Cancel = true;
            }
        }
        #endregion

        #region Delete
        private async Task Conductor_BeforeDelete(R_BeforeDeleteEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                if (_conductorRef.R_DisableCancelConfirmation)
                {
                    var loResult = await MessageBoxService.Show("Dari Before Delete", "Mau Delete?", R_eMessageBoxButtonType.YesNo);
                    if (loResult == R_eMessageBoxResult.No)
                    {
                        eventArgs.Cancel = true;
                    }
                }

                var loData = (ProductDTO)eventArgs.Data;
                if (loData.Active)
                {
                    eventArgs.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

        }
        private void Conductor_ServiceDelete(R_ServiceDeleteEventArgs eventArgs)
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
        private async Task Conductor_AfterDelete()
        {
            var loEx = new R_Exception();

            try
            {
                await R_MessageBox.Show("", $"Delete success.", R_eMessageBoxButtonType.OK);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        #endregion

        #region Cancel
        private async Task Conductor_BeforeCancel(R_BeforeCancelEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                if (_conductorRef.R_DisableCancelConfirmation)
                {
                    var loResult = await MessageBoxService.Show("Dari Before Cancel", "Mau Cancel?", R_eMessageBoxButtonType.YesNo);
                    if (loResult == R_eMessageBoxResult.No)
                    {
                        eventArgs.Cancel = true;
                    }
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        #endregion

        #region Save
        private void Conductor_Validation(R_ValidationEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                ViewModel.Validation((ProductDTO)eventArgs.Data);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        private void Conductor_Saving(R_SavingEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loData = (ProductDTO)eventArgs.Data;
                loData.ReleaseDate = ViewModel.ReleaseDate;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        private void Conductor_ServiceSave(R_ServiceSaveEventArgs eventArgs)
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
        private void Conductor_AfterSave(R_AfterSaveEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loData = (ProductDTO)eventArgs.Data;

                if (eventArgs.ConductorMode == R_eConductorMode.Add)
                    R_MessageBox.Show("", $"Add {loData.Id} success.", R_eMessageBoxButtonType.OK);
                else
                    R_MessageBox.Show("", $"Edit {loData.Id} success.", R_eMessageBoxButtonType.OK);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        #endregion

        #region Set
        private void Conductor_SetAddEdit(R_SetEventArgs eventArgs)
        {
            //_isInCRUDMode = eventArgs.Enable;
        }
        private void Conductor_SetOther(R_SetEventArgs eventArgs)
        {
            _isInCRUDMode = !eventArgs.Enable;
        }
        private void Conductor_SetHasData(R_SetEventArgs eventArgs)
        {
            _hasData = eventArgs.Enable;
        }
        #endregion
        #endregion

        #region Tab
        private void R_Before_Open_TabPage(R_BeforeOpenTabPageEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(SAB00901);
            var loParam = _conductorRef.R_GetCurrentData();
            eventArgs.Parameter = loParam;
        }
        private async Task R_After_Open_TabPage(R_AfterOpenTabPageEventArgs eventArgs)
        {
            if (eventArgs.Result is null) return;
            await _conductorRef.R_SetCurrentData(eventArgs.Result);
        }
        #endregion

        #region Find
        public int Count = 0;
        private void R_Before_Open_Find(R_BeforeOpenFindEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(ProductPage);
            eventArgs.Parameter = "Dari Find";
            eventArgs.PageTitle = "Title dari event argument";
        }
        private void R_FindModel(R_FindModelEventArgs eventArgs)
        {
            if (Count % 3 == 0)
            {
                eventArgs.FindModel = R_eFindModel.NoDisplay;
            }
            else if (Count % 3 == 1)
            {
                eventArgs.FindModel = R_eFindModel.ViewOnly;
            }
            else
            {
                eventArgs.FindModel = R_eFindModel.Normal;
            }
            Count++;
        }
        public async Task R_After_Open_Find(R_AfterOpenFindEventArgs eventArgs)
        {
            if (eventArgs.Result == null)
                return;

            var loData = (ProductDTO)eventArgs.Result;
            var loParam = new ProductDTO { Id = loData.Id };

            //await _conductorRef.R_GetEntity(loParam);

            ViewModel.GetProductById(loData.Id);
            await _conductorRef.R_SetCurrentData(ViewModel.Product);
        }
        #endregion

        #region Lookup
        private void R_Before_Open_Lookup(R_BeforeOpenLookupEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(ProductPage);
            eventArgs.Parameter = "Dari Lookup";
            eventArgs.PageTitle = "Title dari event argument";
        }
        public async Task R_After_Open_Lookup(R_AfterOpenLookupEventArgs eventArgs)
        {
            if (eventArgs.Result is null) return;
            await _conductorRef.R_SetCurrentData(eventArgs.Result);
        }
        #endregion

        #region Popup
        public void R_Before_Open_Popup(R_BeforeOpenPopupEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(SAB00901);
            eventArgs.Parameter = _conductorRef.R_GetCurrentData();
        }
        public async Task R_After_Open_Popup(R_AfterOpenPopupEventArgs eventArgs)
        {
            if (!eventArgs.Success || eventArgs.Result is null) return;
            await _conductorRef.R_SetCurrentData(eventArgs.Result);
        }

        public void R_Before_Open_Popup_From_Namespace(R_BeforeOpenPopupEventArgs eventArgs)
        {
            eventArgs.PageNamespace = "SAB00900Front.SAB00901";
            eventArgs.Parameter = _conductorRef.R_GetCurrentData();
        }

        public async Task R_After_Open_Popup_From_Namespace(R_AfterOpenPopupEventArgs eventArgs)
        {
            if (!eventArgs.Success || eventArgs.Result is null) return;
            await _conductorRef.R_SetCurrentData(eventArgs.Result);
        }
        #endregion

        #region Detail
        private void R_Before_Open_Detail(R_BeforeOpenDetailEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(SAB00901);
            eventArgs.Parameter = _conductorRef.R_GetCurrentData();
        }
        private async Task R_After_Open_Detail(R_AfterOpenDetailEventArgs eventArgs)
        {
            if (eventArgs.Result is null) return;
            await _conductorRef.R_SetCurrentData(eventArgs.Result);
        }

        private void R_Before_Open_Detail_From_Namespace(R_BeforeOpenDetailEventArgs eventArgs)
        {
            eventArgs.PageNamespace = "SAB00900Front.SAB00901";
            eventArgs.Parameter = _conductorRef.R_GetCurrentData();
        }
        private async Task R_After_Open_Detail_From_Namespace(R_AfterOpenDetailEventArgs eventArgs)
        {
            if (eventArgs.Result is null) return;
            await _conductorRef.R_SetCurrentData(eventArgs.Result);
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

        #region Button Event
        public async Task buttonCloseOnClick()
        {
            await this.CloseProgramAsync();
        }
        private async Task OnClickPrint()
        {
            var saveFileName = $"{Guid.NewGuid().ToString()}.docx";

            var loByteFile = _fileConverter.R_GetByteFromHtmlString($"<b>{ViewModel.Data.Id}</b>", R_eDocumentType.Docx); //kalo mau save langsung jadi file

            await JS.downloadFileFromStreamHandler(saveFileName, loByteFile);
        }
        #region Lookup
        [Inject] public R_LookupService LookupService { get; set; }
        private async Task lookupButtonOnClick()
        {
            var loEx = new R_Exception();

            try
            {
                var loLookupSettings = new R_LookupSettings
                {
                    PageTitle = "title dari lookup settings"
                };

                var loResult = await LookupService.Show(typeof(ProductPage), "Dari LookupService", loLookupSettings);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        #endregion

        #region Popup
        [Inject] public R_MessageBoxService MessageBoxService { get; set; }
        [Inject] public R_PopupService PopupService { get; set; }
        private async Task popupButtonOnClick()
        {
            var loEx = new R_Exception();

            try
            {
                //var leResult = await MessageBoxService.Show("test", "test", R_eMessageBoxButtonType.OK);

                //if (leResult == R_eMessageBoxResult.OK)
                //{
                //var loResult = await PopupService.Show(typeof(TabTest), "Dari PopupService");
                var loPopupSettings = new R_PopupSettings
                {
                    PageTitle = "Title dari popup settings",
                    WithLock = true,
                    Page = this
                };

                var loResult = await PopupService.Show(typeof(SAB00900), "Dari PopupService", poPopupSettings: loPopupSettings);
                //}

                //var loResult = await PopupService.Show(typeof(ProductPage), "Dari PopupService");
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        #endregion
        #endregion

        #region Input Event
        private void DatePicker_ValueChanged(DateTime? pdValue)
        {
            if (pdValue.HasValue)
            {
                ViewModel.ReleaseDate = pdValue.Value;
            }
        }
        #endregion






    }
}
