using DataProvider.DTOs;
using DataProvider.Services;
using Microsoft.AspNetCore.Components;
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
        private SAB01900ProductViewModel _productViewModel = new();
        private SAB01900CategoryViewModel _categoryViewModel = new();
        private R_ConductorGrid _conGridProductRef;
        private R_Grid<ProductDTO> _gridRef;
        public bool _enableComboCategory = true;
        public bool _pageSupplierOnCRUDmode = false;

        [Inject] private IProductService ProductService { get; set; }
        [Inject] private ICategoryService CategoryService { get; set; }

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                _categoryViewModel = new SAB01900CategoryViewModel(CategoryService);
                _productViewModel = new SAB01900ProductViewModel(ProductService);

                await _categoryViewModel.GetComboCategoryListAsync();

                await _gridRef.R_RefreshGrid(_categoryViewModel.CurrentComboboxValue);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task Grid_R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                await _productViewModel.GetProductListByCategoryAsync((int)eventArgs.Parameter);

                eventArgs.ListEntityResult = _productViewModel.Products;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        #region Conductor
        private async Task Grid_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = (ProductDTO)eventArgs.Data;
                await _productViewModel.GetProductByIdAsync(loParam.Id);

                eventArgs.Result = _productViewModel.Product;
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

        private async Task Grid_ServiceSave(R_ServiceSaveEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                await _productViewModel.SaveProductAsync((ProductDTO)eventArgs.Data, eventArgs.ConductorMode);

                eventArgs.Result = _productViewModel.Product;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task Grid_ServiceDelete(R_ServiceDeleteEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loData = (CategoryDTO)eventArgs.Data;
                await _productViewModel.DeleteProductAsync(loData.Id);
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
                await _gridRef.R_RefreshGrid(_categoryViewModel.CurrentComboboxValue);

                if (_tabStrip.ActiveTab.Id == "TabSupplier")
                {
                    await _tabPageSupplier.InvokeRefreshTabPageAsync(_categoryViewModel.CurrentComboboxValue);
                }
            }
        }

        private void R_Before_Open_TabPage(R_BeforeOpenTabPageEventArgs eventArgs)
        {
            //eventArgs.TargetPageType = typeof(SAB01900Supplier);
            //eventArgs.Parameter = _categoryViewModel.CurrentComboboxValue;
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
