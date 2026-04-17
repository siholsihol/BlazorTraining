using DataDummyProvider.DTOs;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_BlazorFrontEnd.Exceptions;

namespace SAB00600Front
{
    public partial class SAB00601 : R_Page
    {
        private SAB00600ViewModel CustomerViewModel = new();
        private R_ConductorGrid _conGridCustomerRef;
        private R_BatchEditGrid<CustomerDTO> _gridRef;
        private int _pageSize = 10;
        [Inject] private R_MessageBoxService MessageBoxService { get; set; }
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
        private void R_RowRender(R_GridRowRenderEventArgs eventArgs)
        {
            var loData = (CustomerDTO)eventArgs.Data;

            if (loData.GenderId == "M")
            {
                eventArgs.RowClass = "myCustomRowFormatting";
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
        private void Grid_BeforeEditCell(R_BeforeEditCellEventArgs eventArgs)
        {
            var loData = eventArgs.Data as CustomerDTO;

            // Kalau GenderId = "M" maka tidak boleh di edit ContactName
            if (loData.GenderId == "M" && eventArgs.ColumnName == nameof(CustomerDTO.ContactName))
            {
                eventArgs.Cancel = true;
            }
        }
        #endregion

        #region Delete
        private void Grid_BeforeDelete(R_BeforeDeleteEventArgs eventArgs)
        {
            //TODO Validation
            //eventArgs.Cancel = true;
        }
        private async Task Grid_AfterDelete()
        {
            await R_MessageBox.Show("Success", "Success Delete", R_eMessageBoxButtonType.OK);
        }
        private void Grid_R_BulkDelete(R_BulkDeleteEventArgs eventArgs)
        {
            // if has male cannot be deleted
            var loList = eventArgs.Data as List<CustomerDTO>;
            var loDeletedList = loList.Where(x => x.Selected);
            if (loDeletedList.Any(x => x.GenderId == "M"))
            {
                eventArgs.Cancel = true;
            }

            eventArgs.Result = loDeletedList;
        }
        #endregion

        #region Save
        private void Grid_R_BeforeSaveBatch(R_BeforeSaveBatchEventArgs eventArgs)
        {
            var loData = eventArgs.Data as List<CustomerDTO>;

            loData = loData.Where(x => x.Selected).ToList();

            var llCancel = false;

            foreach (var item in loData)
            {
                if (string.IsNullOrEmpty(item.CompanyName))
                {
                    llCancel = true;
                    break;
                }
            }
            if (llCancel)
            {
                eventArgs.Cancel = true;
                MessageBoxService.Show("Save Failed", "Ada data dengan Company Name kosong", R_eMessageBoxButtonType.OK);
            }
        }
        private void Grid_R_ServiceSaveBatch(R_ServiceSaveBatchEventArgs eventArgs)
        {
            var loData = eventArgs.Data as List<CustomerDTO>;

            loData = loData.Where(x => x.Selected).ToList();

            CustomerViewModel.SaveBatch(loData);
        }
        private async Task Grid_R_AfterSaveBatch(R_AfterSaveBatchEventArgs eventArgs)
        {
            await MessageBoxService.Show("Success", "Success Save", R_eMessageBoxButtonType.OK);
        }
        #endregion

        #region Cancel
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
        #endregion
        private void R_CellRender(R_GridCellRenderEventArgs eventArgs)
        {
            var lcCompanyName = eventArgs.Value as string;

            if (lcCompanyName.Length < 5)
            {
                eventArgs.CellClass = "myCustomCellFormatting";
            }
        }
        private void R_CheckBoxSelectValueChanged(R_CheckBoxSelectValueChangedEventArgs eventArgs)
        {
            eventArgs.Enabled = true;
        }
    }
}
