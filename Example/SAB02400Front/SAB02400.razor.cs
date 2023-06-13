using DataDummyProvider.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using R_APICommonDTO;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.Grid;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using R_ProcessAndUploadFront;

namespace SAB02400Front
{
    public partial class SAB02400 : R_Page, R_IProcessProgressStatus
    {
        [Inject] public IJSRuntime JS { get; set; }

        private R_ConductorGrid _conGridEmployeeRef;
        private R_Grid<UserDTO> _gridRef;
        private SAB02400ViewModel _viewModel = new();

        protected override Task R_Init_From_Master(object poParameter)
        {
            _viewModel.GetGenderList();

            return Task.CompletedTask;
        }

        private void R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                if (eventArgs.Parameter.GetType() == typeof(byte[]))
                    _viewModel.ReadExcel((byte[])eventArgs.Parameter);
                else
                    _viewModel.GetUserList();

                eventArgs.ListEntityResult = _viewModel.UserList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void R_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            eventArgs.Result = eventArgs.Data;
        }

        private void R_AfterAdd(R_AfterAddEventArgs eventArgs)
        {
            var loData = (UserDTO)eventArgs.Data;

            loData.Id = Guid.NewGuid().ToString();
            loData.FirstName = "Sihol";
            loData.Gender = 1;
        }

        #region Upload
        private async Task OnChange(InputFileChangeEventArgs args)
        {
            var loEx = new R_Exception();

            try
            {
                //import from excel
                var loMS = new MemoryStream();
                await args.File.OpenReadStream().CopyToAsync(loMS);
                var loFileByte = loMS.ToArray();

                await _gridRef.R_RefreshGrid(loFileByte);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task OnClickBatch()
        {
            await _conGridEmployeeRef.R_SaveBatch();
        }

        private void R_BeforeSaveBatch(R_BeforeSaveBatchEventArgs events)
        {
            var loData = (List<UserDTO>)events.Data;

            events.Cancel = loData.Count == 0;
        }

        private async Task R_ServiceSaveBatch(R_ServiceSaveBatchEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                //Read Excel
                //var loUploadClient = new R_ProcessAndUploadClient(poProcessProgressStatus: this);

                //var loBatchParameter = new R_BatchParameter()
                //{
                //    COMPANY_ID = ClientHelper.CompanyId,
                //    USER_ID = ClientHelper.UserId,
                //    ClassName = "SAB02400Back.SAB02400Cls",
                //    BigObject = eventArgs.Data
                //};

                //var loData = eventArgs.Data;
                //var liDataCount = ((List<UserDTO>)eventArgs.Data).ToList().Count;
                //await loUploadClient.R_BatchProcess<List<UserDTO>>(loBatchParameter, liDataCount);

                //Write Excel
                var loByteFile = _viewModel.WriteExcel((List<UserDTO>)eventArgs.Data);
                var saveFileName = $"{Guid.NewGuid().ToString()}.xlsx";

                await JS.downloadFileFromStreamHandler(saveFileName, loByteFile);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void R_AfterSaveBatch(R_AfterSaveBatchEventArgs eventArgs)
        {

        }

        public Task ProcessComplete(string pcKeyGuid, eProcessResultMode poProcessResultMode)
        {
            return Task.CompletedTask;
        }

        public Task ProcessError(string pcKeyGuid, R_APIException ex)
        {
            return Task.CompletedTask;
        }

        public Task ReportProgress(int pnProgress, string pcStatus)
        {
            return Task.CompletedTask;
        }
        #endregion

        private async Task OnClickGenerate()
        {
            await _gridRef.R_RefreshGrid("generate");
        }

        private async Task OnClickGrouping()
        {
            var loGroupDescriptor = new List<R_GridGroupDescriptor>()
            {
                new R_GridGroupDescriptor() {FieldName = "Gender"}
            };

            await _gridRef.R_GroupBy(loGroupDescriptor);
        }

        //private async Task OnClickErrorSP()
        //{
        //    var loEx = new R_Exception();

        //    try
        //    {
        //        ContextHeader.R_Context.R_SetContext("TEST", new UserDTO { Id = "aaa", FirstName = "sihol" });
        //        await _viewModel.SampleErrorMultiLangAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        loEx.Add(ex);
        //    }

        //    loEx.ThrowExceptionIfErrors();
        //}
    }
}
