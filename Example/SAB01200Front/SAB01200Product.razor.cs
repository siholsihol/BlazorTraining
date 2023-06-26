using DataDummyProvider.DTOs;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Exceptions;

namespace SAB01200Front
{
    public partial class SAB01200Product
    {
        private SAB01200ProductViewModel ProductViewModel = new();
        private R_ConductorGrid _conGridProductRef;
        private R_Grid<ProductDTO> _gridRef;

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                await _gridRef.R_RefreshGrid(poParameter);
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
    }
}
