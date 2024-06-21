using DataDummyProvider.DTOs;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;
using SAB01300Front.DTOs;

namespace SAB01300Front
{
    public partial class SAB01300
    {
        private SAB01300ViewModel _viewModel = new();
        private R_Conductor _conductorRef;
        private R_Grid<CategoryGridDTO> _gridRef;

        private R_Grid<ProductDTO> _productGridRef;
        private R_ConductorGrid _conGridProductRef;

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                await _gridRef.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void GridCategory_R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                _viewModel.GetCategoryList();

                eventArgs.ListEntityResult = _viewModel.CategoryList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void Navigator_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = R_FrontUtility.ConvertObjectToObject<CategoryDTO>(eventArgs.Data);
                _viewModel.GetCategoryById(loParam.Id);

                eventArgs.Result = _viewModel.Category;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void Navigator_Validation(R_ValidationEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loData = (CategoryDTO)eventArgs.Data;

                if (string.IsNullOrWhiteSpace(loData.Name))
                    loEx.Add("", "Please fill Category Name.");
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void Navigator_ServiceSave(R_ServiceSaveEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = (CategoryDTO)eventArgs.Data;
                _viewModel.SaveCategory(loParam, (eCRUDMode)eventArgs.ConductorMode);

                eventArgs.Result = _viewModel.Category;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void Navigator_ServiceDelete(R_ServiceDeleteEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = (CategoryDTO)eventArgs.Data;
                _viewModel.DeleteCategory(loParam.Id);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void Navigator_ConvertToGridEntity(R_ConvertToGridEntityEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                eventArgs.GridData = R_FrontUtility.ConvertObjectToObject<CategoryGridDTO>(eventArgs.Data);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void Navigator_BeforeEdit(R_BeforeEditEventArgs eventArgs)
        {
            //eventArgs.Cancel = true;
        }

        private bool _gridEnabled = true;
        private void Navigator_SetOther(R_SetEventArgs eventArgs)
        {
            _gridEnabled = eventArgs.Enable;
        }

        private R_TextBox _textboxNameRef;
        private async Task Navigator_AfterAdd()
        {
            await _textboxNameRef.FocusAsync();
        }

        private async Task Navigator_Display(R_DisplayEventArgs eventArgs)
        {
            var loCurrentCategory = (CategoryDTO)eventArgs.Data;

            await _productGridRef.R_RefreshGrid(loCurrentCategory.Id);
        }

        #region PRODUCT
        private void GridProduct_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var liCategoryId = (Int32)eventArgs.Parameter;
                _viewModel.GetProductsByCategory(liCategoryId);

                eventArgs.ListEntityResult = _viewModel.Products;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void Batch_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            eventArgs.Result = eventArgs.Data;
        }

        private void Batch_AfterAdd(R_AfterAddEventArgs eventArgs)
        {
            var loData = (ProductDTO)eventArgs.Data;

            loData.ReleaseDate = DateTime.Now;
        }
        #endregion

        private void R_RowRender(R_GridRowRenderEventArgs eventArgs)
        {
            var loData = (CategoryGridDTO)eventArgs.Data;

            if (loData.Id == 2)
            {
                eventArgs.RowClass = "myCustomRowFormatting";
            }
        }
    }
}
