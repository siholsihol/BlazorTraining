using R_APICommonDTO;
using R_CommonFrontBackAPI;
using R_ProcessAndUploadFront;

namespace BatchAndExcel.Pages
{
    public class UploadFileViewModel : R_IProcessProgressStatus
    {
        public Action<R_APIException> ShowErrorAction { get; set; }
        public Action StateChangeAction { get; set; }
        public Action ShowSuccessAction { get; set; }
        public string Message { get; set; }
        public int Percentage { get; set; }

        public Task ProcessComplete(string pcKeyGuid, eProcessResultMode poProcessResultMode)
        {
            if (poProcessResultMode == eProcessResultMode.Success)
            {
                Message = string.Format("Process Complete and success with GUID {0}", pcKeyGuid);
                ShowSuccessAction();
            }

            if (poProcessResultMode == eProcessResultMode.Fail)
            {
                Message = string.Format("Process Complete but fail with GUID {0}", pcKeyGuid);

                //var loResult = await ServiceGetError(pcKeyGuid);
            }

            StateChangeAction();

            return Task.CompletedTask;
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

            //_percentage = lnProgress.ToString() + "%";
            Percentage = pnProgress;
            Message = string.Format("Process Progress {0} with status {1}", pnProgress, pcStatus);

            StateChangeAction();

            return Task.CompletedTask;
        }

        //private async Task<List<R_ErrorStatusReturn>> ServiceGetError(string pcKeyGuid)
        //{
        //    R_APIException loException = new R_APIException();

        //    List<R_ErrorStatusReturn> loResultData = null;
        //    R_GetErrorWithMultiLanguageParameter loParameterData;
        //    R_ProcessAndUploadClient loCls;

        //    try
        //    {
        //        // Add Parameter
        //        loParameterData = new R_GetErrorWithMultiLanguageParameter()
        //        {
        //            COMPANY_ID = loCompany.CCOMPANY_ID,
        //            USER_ID = SelectedUserId,
        //            KEY_GUID = pcKeyGuid,
        //            RESOURCE_NAME = "RSP_UPLOAD_GSM_CENTERResources"
        //        };

        //        loCls = new R_ProcessAndUploadClient(pcModuleName: "GS",
        //            plSendWithContext: true,
        //            plSendWithToken: true,
        //            pcHttpClientName: "R_DefaultServiceUrl");

        //        // Get error result
        //        loResultData = await loCls.R_GetStreamErrorProcess(loParameterData);

        //        loUploadCenterDisplayList.ToList().ForEach(x =>
        //        {
        //            if (loResultData.Any(y => y.SeqNo == x.INO))
        //            {
        //                x.CNOTES = loResultData.Where(y => y.SeqNo == x.INO).FirstOrDefault().ErrorMessage;
        //                x.CVALID = "N";
        //                SumInvalid++;
        //            }
        //            else
        //            {
        //                x.CVALID = "Y";
        //                SumValid++;
        //            }
        //        });

        //        if (loResultData.Any(x => x.SeqNo < 0))
        //        {
        //            var loUnhandleEx = loResultData.Where(x => x.SeqNo < 0).Select(x => new R_BlazorFrontEnd.Exceptions.R_Error(x.SeqNo.ToString(), x.ErrorMessage)).ToList();
        //            var loEx = new R_Exception();
        //            loUnhandleEx.ForEach(x => loEx.Add(x));

        //            loException = R_FrontUtility.R_ConvertToAPIException(loEx);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        loException.add(ex);
        //    }

        //    loException.ThrowExceptionIfErrors();
        //    return loResultData;
        //}
    }
}
