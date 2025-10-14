using DataDummyProvider.DTOs;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using SAB00900Front;

namespace SAB00600Front
{
    public partial class SAB00600 : R_Page
    {
        private SAB00600ViewModel CustomerViewModel = new();
        private R_ConductorGrid _conGridCustomerRef;
        private R_Grid<CustomerDTO> _gridRef;
        private int _pageSize = 10;
        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                CustomerViewModel.GetGenders();

                await _gridRef.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrl";

        //protected override async Task<bool> R_LockUnlock(R_LockUnlockEventArgs eventArgs)
        //{
        //    var loEx = new R_Exception();
        //    var llRtn = false;
        //    R_LockingFrontResult loLockResult = null;

        //    try
        //    {
        //        var loData = (CustomerDTO)eventArgs.Data;

        //        if (eventArgs.Mode == R_eLockUnlock.Lock)
        //        {
        //            var loLockPar = new R_ServiceLockingLockParameterDTO
        //            {
        //                Company_Id = "001",
        //                User_Id = "cp",
        //                Program_Id = "SAB00600",
        //                Table_Name = "TEST_TABLE",
        //                Key_Value = string.Join("|", "001", "cp", loData.Id)
        //            };

        //            var loCls = new R_LockingServiceClient(DEFAULT_HTTP_NAME);

        //            loLockResult = await loCls.R_Lock(loLockPar);
        //        }
        //        else
        //        {
        //            var loUnlockPar = new R_ServiceLockingUnLockParameterDTO
        //            {
        //                Company_Id = "001",
        //                User_Id = "cp",
        //                Program_Id = "SAB00600",
        //                Table_Name = "TEST_TABLE",
        //                Key_Value = string.Join("|", "001", "cp", loData.Id)
        //            };

        //            var loCls = new R_LockingServiceClient(DEFAULT_HTTP_NAME);

        //            loLockResult = await loCls.R_UnLock(loUnlockPar);
        //        }

        //        llRtn = loLockResult.IsSuccess;
        //        if (!loLockResult.IsSuccess && loLockResult.Exception != null)
        //            throw loLockResult.Exception;
        //    }
        //    catch (Exception ex)
        //    {
        //        loEx.Add(ex);
        //    }

        //    loEx.ThrowExceptionIfErrors();

        //    return llRtn;
        //}

        #region Conductor Grid Events
        private void Grid_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
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
        private void R_Display(R_DisplayEventArgs eventArgs)
        {

        }
        private void R_RowRender(R_GridRowRenderEventArgs eventArgs)
        {
            var loData = (CustomerDTO)eventArgs.Data;

            if (loData.GenderId == "M")
            {
                eventArgs.RowClass = "myCustomRowFormatting";
            }
        }
        private void R_CellValueChanged(R_CellValueChangedEventArgs eventArgs)
        {
            if (eventArgs.ColumnName == "CompanyName")
            {
                var loData = eventArgs.CurrentRow as CustomerDTO;
                loData.ContactName = eventArgs.Value as string;
            }

            if (eventArgs.ColumnName == "GenderId")
            {
                var loContactNameColumn = eventArgs.Columns.FirstOrDefault(x => x.Name == "ContactName");
                loContactNameColumn.Enabled = eventArgs.Value.ToString() != "F";
            }
        }
        private void R_CellLostFocus(R_CellLostFocusedEventArgs eventArgs)
        {
            if (eventArgs.ColumnName == nameof(CustomerDTO.ProductId))
            {
                var loData = eventArgs.CurrentRow as CustomerDTO;
                ProductPageViewModel loProductPageViewModel = new ProductPageViewModel();

                var loResult = loProductPageViewModel.GetProductById(loData.ProductId);
                if (loResult != null)
                {
                    loData.ProductId = loResult.Id;
                    loData.ProductName = loResult.Name;
                }
            }
        }
        private void R_Before_Open_Grid_Lookup(R_BeforeOpenGridLookupColumnEventArgs eventArgs)
        {
            //eventArgs.TargetPageType = typeof(ProductPage);
            eventArgs.PageNamespace = "SAB00900Front.ProductPage";
        }
        private void R_After_Open_Grid_Lookup(R_AfterOpenGridLookupColumnEventArgs eventArgs)
        {
            if (eventArgs.ColumnName == nameof(CustomerDTO.ProductId))
            {
                var loData = (CustomerDTO)eventArgs.ColumnData;
                var loResult = (ProductDTO)eventArgs.Result;

                loData.ProductId = loResult.Id;
                loData.ProductName = loResult.Name;
            }
        }

        #region Add
        private void Grid_BeforeAdd(R_BeforeAddEventArgs eventArgs)
        {
            //TODO Validation
            //eventArgs.Cancel = true;
        }
        private void Grid_AfterAdd(R_AfterAddEventArgs eventArgs)
        {
            var loData = (CustomerDTO)eventArgs.Data;
            //loData.Id = "SIHOL";
            //loData.CompanyName = "Realta";
        }
        #endregion

        #region Edit
        private void Grid_BeforeEdit(R_BeforeEditEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                //TODO Validation
                //eventArgs.Cancel = true;

                //throw new Exception("ea");
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        #endregion

        #region Delete
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
        #endregion

        #region Save
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
        #endregion

        #region Cancel
        private void Grid_BeforeCancel(R_BeforeCancelEventArgs eventArgs)
        {
            //TODO Validation
            //eventArgs.Cancel = true;
        }
        #endregion

        #region CHECK EVENT
        private void R_CheckAdd(R_CheckAddEventArgs eventArgs)
        {
            //TODO Validation
            //var loCurrentData = _gridRef.GetCurrentData() as CustomerDTO;
            //eventArgs.Allow = loCurrentData.GenderId == "M";
        }
        private void R_CheckEdit(R_CheckEditEventArgs eventArgs)
        {
            //TODO Validation
            //eventArgs.Allow = false;

            var item = (CustomerDTO)eventArgs.Data;
            var index = _gridRef.DataSource.IndexOf(item);
            if (index % 2 == 0)
            {
                eventArgs.Allow = true;
            }
            else
            {
                eventArgs.Allow = false;
            }
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
            //var loList = (ObservableCollection<CustomerDTO>)eventArgs.DataList;
            //if (loList.Where(x => x.GenderId == "M").ToList().Count > 0)
            //{
            //    eventArgs.Allow = false;
            //}

            //foreach (var item in loList)
            //{
            //    if (item.GenderId == "M")
            //    {
            //        eventArgs.Allow = false;
            //        break;
            //    }
            //}
        }
        private void R_CheckGridDelete(R_CheckGridEventArgs eventArgs)
        {
            //TODO Validation
            //eventArgs.Allow = false;
        }
        #endregion
        #region SET GRID COLUMN
        private void R_SetAddGridColumn(R_SetAddGridColumnEventArgs eventArgs)
        {
            //var loColumn = eventArgs.Columns.FirstOrDefault(x => x.FieldName == "CompanyName");
            //var loData = (CustomerDTO)eventArgs.Data;

            //if (!string.IsNullOrWhiteSpace(loData.CompanyName))
            //{
            //    loColumn.Enabled = false;
            //}
        }
        private void R_SetEditGridColumn(R_SetEditGridColumnEventArgs eventArgs)
        {
            //var loColumn = eventArgs.Columns.FirstOrDefault(x => x.FieldName == "GenderId");

            //if (loColumn != null)
            //{
            //    loColumn.Enabled = false;
            //}

            //var loColumn = eventArgs.Columns.FirstOrDefault(x => x.FieldName == "CompanyName");
            //var loData = (CustomerDTO)eventArgs.Data;

            //if (!string.IsNullOrWhiteSpace(loData.CompanyName))
            //{
            //    loColumn.Enabled = false;
            //}
        }
        #endregion
        #endregion

        private bool _checkBoxValue = true;
        private bool _isContactNameColumnVisible = true;
        private bool _isAddNewRowVisible = true;

        private async Task OnClick()
        {
            //await _gridRef.SaveAsync();
            //var a = _gridRef.GetFilteredGridData();
            //_prefixText = "Mr." + _valuePrefix.ToString();
            _suffixText = _valuePrefix.ToString() + "Kg";
            _valuePrefix += 1;

            await Task.Delay(3000);
        }

        private string _prefixText = "Mr.";
        private int _valuePrefix = 0;
        private string _suffixText = "Kg";
        private void R_CellRender(R_GridCellRenderEventArgs eventArgs)
        {
            var lcCompanyName = eventArgs.Value as string;

            if (lcCompanyName?.Length < 5)
            {
                eventArgs.CellClass = "myCustomCellFormatting";
            }
        }

        private void R_InstantiateDock(R_InstantiateDockEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(SAB00601);
        }
    }
}
