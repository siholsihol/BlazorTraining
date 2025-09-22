using DataProvider.DTOs;
using DataProvider.Services;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
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
        [Inject] public R_IFileConverter FileConverter { get; set; } = default!;
        [Inject] private IProductService ProductService { get; set; } = default!;
        [Inject] private ICategoryService CategoryService { get; set; } = default!;
        [Inject] public R_LookupService LookupService { get; set; } = default!;
        [Inject] public R_PopupService PopupService { get; set; } = default!;
        [Inject] public R_MessageBoxService MessageBoxService { get; set; } = default!;

        private SAB00900ViewModel _viewModel = new();
        private R_Conductor _conductorRef = default!;
        private R_CheckBox _checkboxActiveRef = default!;

        protected override async Task R_Init_From_Master(object? poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                _viewModel = new SAB00900ViewModel(ProductService, CategoryService);

                await _viewModel.GetCategories();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task Conductor_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                if (eventArgs.Data == null)
                    return;

                var loParam = (ProductDTO)eventArgs.Data;
                await _viewModel.GetProductById(loParam.Id);

                eventArgs.Result = _viewModel.Product;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task Conductor_AfterAdd(R_AfterAddEventArgs eventArgs)
        {
            var loEntity = (ProductDTO)eventArgs.Data;

            loEntity.CategoryId = 1;

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
                    loEx.Add("", "Please fill Product Name.");
                }

                if (loData.Price == 0)
                {
                    loEx.Add("", "Please fill Price.");
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            if (loEx.HasError)
                eventArgs.Cancel = true;

            loEx.ThrowExceptionIfErrors();
        }

        public async Task Conductor_ServiceSave(R_ServiceSaveEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = (ProductDTO)eventArgs.Data;
                await _viewModel.SaveProduct(loParam, eventArgs.ConductorMode);

                eventArgs.Result = _viewModel.Product;
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

        public async Task Conductor_ServiceDelete(R_ServiceDeleteEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = (ProductDTO)eventArgs.Data;
                await _viewModel.DeleteProduct(loParam.Id);
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

        //private int _count = 0;
        public void R_FindModel(R_FindModelEventArgs eventArgs)
        {
            //if (_count % 2 == 0)
            //{
            //    eventArgs.FindModel = R_eFindModel.NoDisplay;
            //}
            //else if (_count % 2 == 1)
            //{
            //    eventArgs.FindModel = R_eFindModel.ViewOnly;
            //}

            //_count++;
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

        public Task R_Before_Open_Popup(R_BeforeOpenPopupEventArgs eventArgs)
        {
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

        private async Task popupButtonOnClick()
        {
            var loEx = new R_Exception();

            try
            {
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

        public void Conductor_BeforeEdit(R_BeforeEditEventArgs eventArgs)
        {
            //var loEx = new R_Exception();

            //try
            //{
            //    var loResult = await PopupService.Show(typeof(ProductPage), "Dari PopupService");

            //    eventArgs.Cancel = !loResult.Success;
            //}
            //catch (Exception ex)
            //{
            //    loEx.Add(ex);
            //}

            //loEx.ThrowExceptionIfErrors();
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

        public void Conductor_BeforeCancel()
        {
        }

        public void Conductor_Display(R_DisplayEventArgs eventArgs)
        {
        }

        //protected override Task<bool> R_LockUnlock(R_LockUnlockEventArgs eventArgs)
        //{
        //    return Task.FromResult(false);
        //}

        #region Detail

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

        #endregion

        private async Task OnClickPrint()
        {
            var saveFileName = $"{Guid.NewGuid().ToString()}.docx";

            var loByteFile = FileConverter.R_GetByteFromHtmlString($"<b>{_viewModel.Data.Id}</b>", R_eDocumentType.Docx); //kalo mau save langsung jadi file

            if (loByteFile != null)
                await JS.downloadFileFromStreamHandler(saveFileName, loByteFile);
        }
    }
}
