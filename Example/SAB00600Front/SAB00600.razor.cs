using DataDummyProvider.DTOs;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.Grid;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;

namespace SAB00600Front
{
    public partial class SAB00600 : R_Page
    {
        private SAB00600ViewModel CustomerViewModel = new();

        private R_ConductorGrid _conGridCustomerRef;

        private R_Grid<CustomerDTO> _gridRef;

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
                CustomerViewModel.GetCustomerList();

                eventArgs.ListEntityResult = CustomerViewModel.CustomerList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void Grid_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = (CustomerDTO)eventArgs.Data;
                CustomerViewModel.GetCustomerById(loParam.Id);

                eventArgs.Result = CustomerViewModel.Customer;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void Grid_BeforeEdit(R_BeforeEditEventArgs eventArgs)
        {
            //TODO Validation
            //eventArgs.Cancel = true;
        }

        private void Grid_BeforeCancel(R_BeforeCancelEventArgs eventArgs)
        {
            //TODO Validation
            //eventArgs.Cancel = true;
        }

        private void Grid_BeforeAdd(R_BeforeAddEventArgs eventArgs)
        {
            //TODO Validation
            //eventArgs.Cancel = true;
        }

        private void Grid_AfterAdd(R_AfterAddEventArgs eventArgs)
        {
            var loData = (CustomerDTO)eventArgs.Data;
            loData.Id = "SIHOL";
            loData.CompanyName = "Realta";
        }

        private void Grid_Validation(R_ValidationEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loData = (CustomerDTO)eventArgs.Data;

                if (string.IsNullOrWhiteSpace(loData.Id))
                    loEx.Add("001", "Customer Id cannot be null.");
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            eventArgs.Cancel = loEx.HasError;

            loEx.ThrowExceptionIfErrors();
        }

        private void Grid_Saving(R_SavingEventArgs eventArgs)
        {
            if (eventArgs.ConductorMode == R_eConductorMode.Add)
            {
                var loData = (CustomerDTO)eventArgs.Data;
                loData.Address = "Depok";
            }
        }

        private void Grid_ServiceSave(R_ServiceSaveEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                CustomerViewModel.SaveCustomer((CustomerDTO)eventArgs.Data, (eCRUDMode)eventArgs.ConductorMode);

                eventArgs.Result = CustomerViewModel.Customer;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task Grid_AfterSave(R_AfterSaveEventArgs eventArgs)
        {
            await R_MessageBox.Show("Success", "Success", R_eMessageBoxButtonType.OK);
        }

        private void Grid_BeforeDelete(R_BeforeDeleteEventArgs eventArgs)
        {
            //TODO Validation
            //eventArgs.Cancel = true;
        }

        private void Grid_ServiceDelete(R_ServiceDeleteEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loData = (CustomerDTO)eventArgs.Data;
                CustomerViewModel.DeleteCustomer(loData.Id);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task Grid_AfterDelete()
        {
            await R_MessageBox.Show("Success", "Success Delete", R_eMessageBoxButtonType.OK);
        }

        private void R_Display(R_DisplayEventArgs eventArgs)
        {

        }

        #region CHECK EVENT
        private void R_CheckAdd(R_CheckAddEventArgs eventArgs)
        {
            //TODO Validation
            //eventArgs.Allow = false;
        }

        private void R_CheckEdit(R_CheckEditEventArgs eventArgs)
        {
            //TODO Validation
            //eventArgs.Allow = false;
        }

        private void R_CheckDelete(R_CheckDeleteEventArgs eventArgs)
        {
            //TODO Validation
            //eventArgs.Allow = false;
        }
        #endregion

        #region CHECK GRID EVENT
        private void R_CheckGridAdd(R_CheckGridEventArgs eventArgs)
        {
            //TODO Validation
            //eventArgs.Allow = false;
        }

        private void R_CheckGridEdit(R_CheckGridEventArgs eventArgs)
        {
            //TODO Validation
            //eventArgs.Allow = false;
        }

        private void R_CheckGridDelete(R_CheckGridEventArgs eventArgs)
        {
            //TODO Validation
            //eventArgs.Allow = false;
        }
        #endregion

        private void R_RowRender(R_GridRowRenderEventArgs eventArgs)
        {
            var loData = (CustomerDTO)eventArgs.Data;

            if (loData.GenderId == "M")
            {
                eventArgs.RowStyle = new R_GridRowRenderStyle
                {
                    FontColor = "red"
                };
            }
        }
    }
}
