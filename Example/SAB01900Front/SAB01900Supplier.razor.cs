using DataDummyProvider.DTOs;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.Tab;
using R_BlazorFrontEnd.Exceptions;
using SAB01900Front.VMs;

namespace SAB01900Front
{
    public partial class SAB01900Supplier : R_ITabPage
    {
        private SAB01900SupplierViewModel SupplierViewModel = new();
        private R_ConductorGrid _conGridProductRef;
        private R_Grid<SupplierDTO> _gridRef;

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                await _gridRef.R_RefreshGrid((int)poParameter);
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
                SupplierViewModel.GetSuppliersByCategory((int)eventArgs.Parameter);

                eventArgs.ListEntityResult = SupplierViewModel.Suppliers;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task Grid_R_SetOther(R_SetEventArgs eventArgs)
        {
            await InvokeTabEventCallbackAsync(eventArgs.Enable);
        }

        public async Task RefreshTabPageAsync(object poParam)
        {
            await _gridRef.R_RefreshGrid((int)poParam);
        }

        protected override Task<object> R_Set_Result_TabPage()
        {
            return Task.FromResult<object>("Result Tab Supplier");
        }

        #region Conductor
        private void Grid_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            //var loEx = new R_Exception();

            //try
            //{
            //    var loParam = (ProductDTO)eventArgs.Data;
            //    SupplierViewModel.GetProductById(loParam.Id);

            //    eventArgs.Result = SupplierViewModel.Product;
            //}
            //catch (Exception ex)
            //{
            //    loEx.Add(ex);
            //}

            //loEx.ThrowExceptionIfErrors();
        }

        private void Grid_Validation(R_ValidationEventArgs eventArgs)
        {
            //var loEx = new R_Exception();

            //try
            //{
            //    var loData = (ProductDTO)eventArgs.Data;

            //    if (string.IsNullOrWhiteSpace(loData.Name))
            //        loEx.Add("001", "Product Name cannot be null.");
            //}
            //catch (Exception ex)
            //{
            //    loEx.Add(ex);
            //}

            //eventArgs.Cancel = loEx.HasError;

            //loEx.ThrowExceptionIfErrors();
        }

        private void Grid_ServiceSave(R_ServiceSaveEventArgs eventArgs)
        {
            //var loEx = new R_Exception();

            //try
            //{
            //    SupplierViewModel.SaveProduct((ProductDTO)eventArgs.Data, eventArgs.ConductorMode);

            //    eventArgs.Result = SupplierViewModel.Product;
            //}
            //catch (Exception ex)
            //{
            //    loEx.Add(ex);
            //}

            //loEx.ThrowExceptionIfErrors();
        }

        private void Grid_ServiceDelete(R_ServiceDeleteEventArgs eventArgs)
        {
            //var loEx = new R_Exception();

            //try
            //{
            //    var loData = (CategoryDTO)eventArgs.Data;
            //    SupplierViewModel.DeleteProduct(loData.Id);
            //}
            //catch (Exception ex)
            //{
            //    loEx.Add(ex);
            //}

            //loEx.ThrowExceptionIfErrors();
        }
        #endregion
    }
}
