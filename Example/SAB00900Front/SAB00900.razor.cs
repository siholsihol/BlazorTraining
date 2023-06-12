using DataDummyProvider.DTOs;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.MessageBox;
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

        public void Conductor_AfterAdd(R_AfterAddEventArgs eventArgs)
        {
            var loEntity = (ProductDTO)eventArgs.Data;

            loEntity.CategoryId = 1;
            loEntity.ReleaseDate = DateTime.Now;
        }

        public void Conductor_Validation(R_ValidationEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loData = (ProductDTO)eventArgs.Data;

                if (string.IsNullOrWhiteSpace(loData.Name))
                    loEx.Add("", "Please fill Product Name.");
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

        #region Find
        public void R_Before_Open_Find(R_BeforeOpenFindEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(ProductPage);
        }

        public async Task R_After_Open_Find(R_AfterOpenFindEventArgs eventArgs)
        {
            var loData = (ProductDTO)eventArgs.Result;
            var loParam = new ProductDTO { Id = loData.Id };

            await _conductorRef.R_GetEntity(loParam);
        }
        #endregion

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
    }
}
