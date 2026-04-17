using R_APICommonDTO;
using R_CommonFrontBackAPI;
using R_ProcessAndUploadFront;

namespace BatchAndExcel.Pages.File
{
    public class FileViewModel : R_IProcessProgressStatus
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
    }
}
