using DataDummyProvider.DTOs;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;
using SAB00700Front.DTOs;

namespace SAB00700Front
{
    public partial class SAB00700
    {
        private SAB00700ViewModel _viewModel = new();
        private R_Conductor _conductorRef;
        private R_Grid<CategoryGridDTO> _gridRef;
        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                await _gridRef.R_RefreshGrid(null);
                //await _conductorRef.Edit();
                //await _gridRef.R_SelectCurrentDataAsync(2);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        //private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrl";

        //protected override async Task<bool> R_LockUnlock(R_LockUnlockEventArgs eventArgs)
        //{
        //    var loEx = new R_Exception();
        //    var llRtn = false;
        //    R_LockingFrontResult loLockResult = null;

        //    try
        //    {
        //        var loData = (CategoryDTO)eventArgs.Data;

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

        private void Grid_R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
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

        private void Conductor_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
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

        private void Conductor_Validation(R_ValidationEventArgs eventArgs)
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

        private void Conductor_ServiceSave(R_ServiceSaveEventArgs eventArgs)
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

        private void Conductor_ServiceDelete(R_ServiceDeleteEventArgs eventArgs)
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

        private void R_ConvertToGridEntity(R_ConvertToGridEntityEventArgs eventArgs)
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

        private void R_Display(R_DisplayEventArgs eventArgs)
        {
            _txtDescEnabled = eventArgs.ConductorMode == R_BlazorFrontEnd.Enums.R_eConductorMode.Normal ? false : true;
        }

        private void R_BeforeEdit(R_BeforeEditEventArgs eventArgs)
        {
            //eventArgs.Cancel = true;
        }

        private bool _enableGroupBox = true;
        private async Task buttonOnClick()
        {
            var loEx = new R_Exception();

            try
            {
                var loData = _conductorRef.R_GetCurrentData() as CategoryDTO; //get current data

                _viewModel.ChangeCategoryName(loData.Id);

                var loCategory = _viewModel.GetCategory(loData.Id);

                await _conductorRef.R_SetCurrentData(loCategory);

                _enableGroupBox = !_enableGroupBox;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private bool _gridEnabled = true;
        private void R_SetOther(R_SetEventArgs eventArgs)
        {
            _gridEnabled = eventArgs.Enable;
        }

        private R_TextBox _textboxNameRef;
        private async Task R_AfterAdd()
        {
            await _textboxNameRef.FocusAsync();
        }

        private void R_CheckAdd(R_CheckAddEventArgs eventArgs)
        {
            //eventArgs.Allow = false;
        }

        private void R_CheckEdit(R_CheckEditEventArgs eventArgs)
        {
            //eventArgs.Allow = false;
        }

        private void R_CheckDelete(R_CheckDeleteEventArgs eventArgs)
        {
            //eventArgs.Allow = false;
        }

        private bool _buttonEnable = false;
        private void R_SetHasData(R_SetEventArgs eventArgs)
        {
            _buttonEnable = eventArgs.Enable;
        }

        private string _editorValue { get; set; }

        private bool _txtDescEnabled;
        private async Task txtNameOnLostFocus(object value)
        {
            var name = _viewModel.Data.Name;

            //Thread.Sleep(3000);

            //Task loTask = delaynih();

            //while (!loTask.IsCompleted)
            //{
            //    Thread.Sleep(2);
            //}

            //if (!loTask.IsCompleted)
            //{
            //    Thread.Sleep(500);
            //}

            await Task.Delay(3000);

            //Task.WaitAny(loTask);

            _txtDescEnabled = string.IsNullOrWhiteSpace(name) ? false : true;
        }

        private async Task delaynih()
        {
            await Task.Delay(3000);
        }
    }
}
