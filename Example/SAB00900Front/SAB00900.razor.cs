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

        protected override Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                //var leResult = await MessageBoxService.Show("test", "test", R_eMessageBoxButtonType.OK);

                ViewModel.GetCategories();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

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
            eventArgs.PageTitle = "Title dari event argument";
        }
        public int Count = 0;
        public async Task R_FindModel(R_FindModelEventArgs eventArgs)
        {
            if (Count % 2 == 0)
            {
                eventArgs.FindModel = R_eFindModel.NoDisplay;
            }
            else if (Count % 2 == 1)
            {
                eventArgs.FindModel = R_eFindModel.ViewOnly;
            }
            Count++;
        }
        public async Task R_After_Open_Find(R_AfterOpenFindEventArgs eventArgs)
        {
            if (eventArgs.Result == null)
                return;

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
            eventArgs.PageTitle = "Title dari event argument";
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
        public Task R_Before_Open_Popup(R_BeforeOpenPopupEventArgs eventArgs)
        {
            //var leResult = await MessageBoxService.Show("test", "test", R_eMessageBoxButtonType.OK);

            //if (leResult == R_eMessageBoxResult.OK)
            //{
            //    eventArgs.TargetPageType = typeof(ProductPage);
            //    eventArgs.Parameter = "Dari Popup";
            //}

            eventArgs.TargetPageType = typeof(ProductPage);
            eventArgs.Parameter = "Dari Popup";

            return Task.CompletedTask;
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
            await this.CloseProgramAsync();
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

        public void Conductor_Display(R_DisplayEventArgs eventArgs)
        {
            var loData = eventArgs.Data as ProductDTO;
            if (loData != null && eventArgs.ConductorMode == R_eConductorMode.Normal)
            {
                ViewModel.ReleaseDate = loData.ReleaseDate;
            }
        }

        protected override Task<bool> R_LockUnlock(R_LockUnlockEventArgs eventArgs)
        {
            return Task.FromResult(false);
        }

        private void R_Before_Open_Detail(R_BeforeOpenDetailEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(ProductPage);
            //eventArgs.PageNamespace = "SAB00600Front.SAB00600";
            eventArgs.Parameter = "From Detail Button";
            eventArgs.PageTitle = "Title dari event argument";
        }

        private void R_After_Open_Detail(R_AfterOpenDetailEventArgs eventArgs)
        {

        }

        private async Task DatePicker_ValueChanged(DateTime? pdValue)
        {
            if (pdValue.HasValue)
            {
                ViewModel.ReleaseDate = pdValue.Value;

                var loResult = await PopupService.Show(typeof(ProductPage), "Dari PopupService");
            }
        }



        private async Task OnClickPrint()
        {
            var saveFileName = $"{Guid.NewGuid().ToString()}.docx";

            var loByteFile = _fileConverter.R_GetByteFromHtmlString($"<b>{ViewModel.Data.Id}</b>", R_eDocumentType.Docx); //kalo mau save langsung jadi file

            await JS.downloadFileFromStreamHandler(saveFileName, loByteFile);
        }
    }
}
