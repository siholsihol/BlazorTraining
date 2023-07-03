using DataDummyProvider.DTOs;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.Tab;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using SAB01900Front.VMs;

namespace SAB01900Front
{
    public partial class SAB01900
    {
        private SAB01900ProductViewModel ProductViewModel = new();
        private SAB01900CategoryViewModel CategoryViewModel = new();
        private R_ConductorGrid _conGridProductRef;
        private R_Grid<ProductDTO> _gridRef;

        public bool _enableComboCategory = true;
        public bool _pageSupplierOnCRUDmode = false;

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                CategoryViewModel.GetComboCategoryList();

                await _gridRef.R_RefreshGrid(CategoryViewModel.CurrentComboboxValue);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void Grid_R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                ProductViewModel.GetProductListByCategory((int)eventArgs.Parameter);

                eventArgs.ListEntityResult = ProductViewModel.Products;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        #region Conductor
        private void Grid_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = (ProductDTO)eventArgs.Data;
                ProductViewModel.GetProductById(loParam.Id);

                eventArgs.Result = ProductViewModel.Product;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void Grid_Validation(R_ValidationEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loData = (ProductDTO)eventArgs.Data;

                if (string.IsNullOrWhiteSpace(loData.Name))
                    loEx.Add("001", "Product Name cannot be null.");
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            eventArgs.Cancel = loEx.HasError;

            loEx.ThrowExceptionIfErrors();
        }

        private void Grid_ServiceSave(R_ServiceSaveEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                ProductViewModel.SaveProduct((ProductDTO)eventArgs.Data, eventArgs.ConductorMode);

                eventArgs.Result = ProductViewModel.Product;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void Grid_ServiceDelete(R_ServiceDeleteEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loData = (CategoryDTO)eventArgs.Data;
                ProductViewModel.DeleteProduct(loData.Id);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        #endregion

        private void OnActiveTabIndexChanging(R_TabStripActiveTabIndexChangingEventArgs eventArgs)
        {
            eventArgs.Cancel = _pageSupplierOnCRUDmode;
        }

        private async Task ComboCategoryOnChanged(object poValue)
        {
            if (_conGridProductRef.R_ConductorMode == R_eConductorMode.Normal)
            {
                await _gridRef.R_RefreshGrid(CategoryViewModel.CurrentComboboxValue);

                if (_tabStrip.ActiveTab.Id == "TabSupplier")
                {
                    await _tabPageSupplier.InvokeRefreshTabPageAsync(CategoryViewModel.CurrentComboboxValue);
                }
            }
        }

        private void R_Before_Open_TabPage(R_BeforeOpenTabPageEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(SAB01900Supplier);
            eventArgs.Parameter = CategoryViewModel.CurrentComboboxValue;
        }

        private void R_After_Open_TabPage(R_AfterOpenTabPageEventArgs eventArgs)
        {

        }

        private bool _comboboxEnabled = true;
        private void R_TabEventCallback(object poValue)
        {
            _comboboxEnabled = (bool)poValue;
            _pageSupplierOnCRUDmode = !(bool)poValue;
        }

        private R_TabPage _tabPageSupplier;
        private R_TabStrip _tabStrip;
    }
}
