using R_APICommonDTO;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;
using R_ProcessAndUploadFront;

namespace BatchAndExcel.ViewModels
{
    public class BatchViewModel : R_IProcessProgressStatus
    {
        public Action<R_APIException> ShowErrorAction { get; set; }
        public Action StateChangeAction { get; set; }
        public Action ShowSuccessAction { get; set; }
        public string Message { get; set; }
        public int Percentage { get; set; }

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

                var loError = await ServiceGetError(pcKeyGuid);
                ShowErrorAction(loError);
            }

            StateChangeAction();
        }

        public Task ProcessError(string pcKeyGuid, R_APIException ex)
        {
            Message = string.Format("Process Error with GUID {0}", pcKeyGuid);

            ShowErrorAction(ex);
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

        private async Task<R_APIException> ServiceGetError(string pcKeyGuid)
        {
            var loAPIEx = new R_APIException();
            List<R_ErrorStatusReturn> loResultData = null;

            try
            {
                // Add Parameter
                //var loParameterMultiLanguage = new R_GetErrorWithMultiLanguageParameter()
                //{
                //    COMPANY_ID = "RCD",
                //    USER_ID = "cp",
                //    KEY_GUID = pcKeyGuid,
                //    RESOURCE_NAME = "RSP_UPLOAD_GSM_CENTERResources"
                //};

                var loParameter = new R_UploadAndProcessKey
                {
                    COMPANY_ID = "RCD",
                    USER_ID = "cp",
                    KEY_GUID = pcKeyGuid
                };

                var loCls = new R_ProcessAndUploadClient(
                    plSendWithContext: true,
                    plSendWithToken: true);

                // Get error result
                //loResultData = await loCls.R_GetStreamErrorProcess(loParameterMultiLanguage);
                loResultData = await loCls.R_GetErrorProcess(loParameter);

                if (loResultData.Count > 0)
                {
                    var loErrorList = loResultData.Select(x => new R_BlazorFrontEnd.Exceptions.R_Error(x.SeqNo.ToString(), x.ErrorMessage));
                    var loEx = new R_Exception();
                    foreach (var loError in loErrorList)
                    {
                        loEx.Add(loError);
                    }

                    loAPIEx = R_FrontUtility.R_ConvertToAPIException(loEx);
                }
            }
            catch (Exception ex)
            {
                loAPIEx.add(ex);
            }

            return loAPIEx;
        }
    }
}
