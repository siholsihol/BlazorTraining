using DataDummyProvider.DTOs;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;

namespace SAB01200Front
{
    public partial class SAB01200
    {
        private SAB01200CategoryViewModel CategoryViewModel = new();
        private R_ConductorGrid _conGridCategoryRef;
        private R_Grid<CategoryDTO> _gridRef;

        private CategoryDTO _tabParameter;

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

        private void Grid_R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                CategoryViewModel.GetCategoryList();

                eventArgs.ListEntityResult = CategoryViewModel.Categories;
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
                var loParam = (CategoryDTO)eventArgs.Data;
                CategoryViewModel.GetCategoryById(loParam.Id);

                eventArgs.Result = CategoryViewModel.Category;
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
                var loData = (CategoryDTO)eventArgs.Data;

                if (string.IsNullOrWhiteSpace(loData.Name))
                    loEx.Add("001", "Category Name cannot be null.");
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
                CategoryViewModel.SaveCategory((CategoryDTO)eventArgs.Data, (eCRUDMode)eventArgs.ConductorMode);

                eventArgs.Result = CategoryViewModel.Category;
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
                CategoryViewModel.DeleteCategory(loData.Id);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void Grid_Display(R_DisplayEventArgs eventArgs)
        {
            if (eventArgs.ConductorMode == R_BlazorFrontEnd.Enums.R_eConductorMode.Normal)
                _tabParameter = (CategoryDTO)eventArgs.Data;
        }
        #endregion
    }
}
