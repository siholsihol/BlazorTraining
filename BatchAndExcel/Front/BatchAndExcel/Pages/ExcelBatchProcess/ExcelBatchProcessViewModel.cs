using BatchAndExcelCommon.DTOs;
using R_APICommonDTO;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;
using R_ProcessAndUploadFront;
using System.Data;

namespace BatchAndExcel.Pages.ExcelBatchProcess
{
    public class ExcelBatchProcessViewModel : R_IProcessProgressStatus
    {
        public Action<DataSet> ShowErrorAction { get; set; }
        public Action StateChangeAction { get; set; }
        public Action ShowSuccessAction { get; set; }
        public string Message { get; set; }
        public int Percentage { get; set; }
        public DataSet EmployeeDataSet { get; private set; }
        public long MaximumFileSize => 5 * 1024 * 1024;

        public async Task SaveBatchEmployeeAsync(DataSet poEmployeeExcelDataSet)
        {
            var loEx = new R_Exception();

            try
            {
                //create dataset
                EmployeeDataSet = poEmployeeExcelDataSet;

                var loEmployeeListFromExcel = R_FrontUtility.R_ConvertTo<EmployeeBatchProcessDTO>(EmployeeDataSet.Tables[0]).ToList();

                var loCls = new R_ProcessAndUploadClient(
                            plSendWithContext: false,
                            plSendWithToken: false,
                            poProcessProgressStatus: this);

                //preapare Batch Parameter
                var loBatchPar = new R_BatchParameter();
                loBatchPar.COMPANY_ID = "RCD";
                loBatchPar.USER_ID = "cp";
                loBatchPar.ClassName = "BatchAndExcelBack.ExcelBatchProcessCls";
                loBatchPar.BigObject = loEmployeeListFromExcel;
                var lcGuid = await loCls.R_BatchProcess<List<EmployeeBatchProcessDTO>>(loBatchPar, 4);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task SaveBatchEmployeeWithResourcesAsync(DataSet poEmployeeExcelDataSet)
        {
            var loEx = new R_Exception();

            try
            {
                //create dataset
                EmployeeDataSet = poEmployeeExcelDataSet;

                var loEmployeeListFromExcel = R_FrontUtility.R_ConvertTo<EmployeeBatchProcessDTO>(EmployeeDataSet.Tables[0]).ToList();

                var loCls = new R_ProcessAndUploadClient(
                            plSendWithContext: false,
                            plSendWithToken: false,
                            poProcessProgressStatus: this);

                //preapare Batch Parameter
                var loBatchPar = new R_BatchParameter();
                loBatchPar.COMPANY_ID = "RCD";
                loBatchPar.USER_ID = "cp";
                loBatchPar.ClassName = "BatchAndExcelBack.ExcelBatchProcessWithResourcesCls";
                loBatchPar.BigObject = loEmployeeListFromExcel;
                var lcGuid = await loCls.R_BatchProcess<List<EmployeeBatchProcessDTO>>(loBatchPar, 4);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        #region PROCESS STATUS

        public async Task ProcessComplete(string pcKeyGuid, eProcessResultMode poProcessResultMode)
        {
            if (poProcessResultMode == eProcessResultMode.Success)
            {
                Message = string.Format("Process Complete and success with GUID {0}", pcKeyGuid);
                ShowSuccessAction();
            }

            if (poProcessResultMode == eProcessResultMode.Fail)
            {
                Message = string.Format("Process Complete but fail with GUID {0}", pcKeyGuid);

                var loErrorDataSet = await ServiceGetError(pcKeyGuid);
                ShowErrorAction(loErrorDataSet);
            }

            StateChangeAction();
        }

        public Task ProcessError(string pcKeyGuid, R_APIException ex)
        {
            Message = string.Format("Process Error with GUID {0}", pcKeyGuid);

            ShowErrorAction(null);
            StateChangeAction();

            return Task.CompletedTask;
        }

        public Task ReportProgress(int pnProgress, string pcStatus)
        {
            Message = string.Format("Process Progress {0} with status {1}", pnProgress, pcStatus);

            Percentage = pnProgress;
            Message = string.Format("Process Progress {0} with status {1}", pnProgress, pcStatus);

            StateChangeAction();

            return Task.CompletedTask;
        }

        private async Task<DataSet> ServiceGetError(string pcKeyGuid)
        {
            var loAPIEx = new R_APIException();
            List<R_ErrorStatusReturn> loResultData = null;
            DataSet loResultDataSet = null;

            try
            {
                // Add Parameter dengan resources
                var loParameterMultiLanguage = new R_GetErrorWithMultiLanguageParameter()
                {
                    COMPANY_ID = "RCD",
                    USER_ID = "cp",
                    KEY_GUID = pcKeyGuid,
                    RESOURCE_NAME = "SaveBatchEmployeeWithResourcesResources"
                };

                // Add Parameter tanpa resources
                var loParameter = new R_UploadAndProcessKey
                {
                    COMPANY_ID = "RCD",
                    USER_ID = "cp",
                    KEY_GUID = pcKeyGuid
                };

                var loCls = new R_ProcessAndUploadClient(
                    plSendWithContext: false,
                    plSendWithToken: false);

                // Get error result
                //loResultData = await loCls.R_GetStreamErrorProcess(loParameterMultiLanguage); //dengan resources
                loResultData = await loCls.R_GetErrorProcess(loParameter); //tanpa multi resources

                if (loResultData.Count > 0)
                {
                    //manipulate dengan resources
                    //var loEmployeeListFromExcel = R_FrontUtility.R_ConvertTo<EmployeeBatchProcessDTO>(EmployeeDataSet.Tables[0]).ToList();

                    //loResultData = loResultData.Select(x => new R_ErrorStatusReturn
                    //{
                    //    SeqNo = x.SeqNo,
                    //    ErrorMessage = string.Format(x.ErrorMessage, loEmployeeListFromExcel.FirstOrDefault(y => y.SeqNo == x.SeqNo)?.EmployeeId)
                    //}).ToList();

                    var loDataTable = R_FrontUtility.R_ConvertTo(loResultData);
                    loDataTable.TableName = "ErrorList";

                    loResultDataSet = EmployeeDataSet.Copy();

                    loResultDataSet.Tables.Add(loDataTable);
                }
            }
            catch (Exception ex)
            {
                loAPIEx.add(ex);
            }

            loAPIEx.ThrowExceptionIfErrors();

            return loResultDataSet;
        }

        #endregion

    }
}
