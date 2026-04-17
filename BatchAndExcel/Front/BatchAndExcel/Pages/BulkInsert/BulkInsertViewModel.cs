using BatchAndExcelCommon.DTOs;
using Bogus;
using R_APICommonDTO;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using R_ProcessAndUploadFront;

namespace BatchAndExcel.Pages.BulkInsert
{
    public class BulkInsertViewModel : R_IProcessProgressStatus
    {
        public Action<R_APIException> ShowErrorAction { get; set; }
        public Action StateChangeAction { get; set; }
        public Action ShowSuccessAction { get; set; }
        public string Message { get; set; }
        public int Percentage { get; set; }

        private List<EmployeeDTO> _employeeList = new();

        public void GenerateEmployeeData()
        {
            var loEx = new R_Exception();

            try
            {
                var loFake = new Faker<EmployeeDTO>()
               .CustomInstantiator(x => new EmployeeDTO())
               .RuleFor(x => x.FirstName, x => x.Name.FirstName())
               .RuleFor(x => x.Gender, x => x.PickRandom(new[] { "M", "F" }))
               .RuleFor(x => x.Id, x => Guid.NewGuid().ToString());

                _employeeList = loFake.Generate(100);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task BulkInsertProcessEmployee()
        {
            var loEx = new R_Exception();

            try
            {
                //instantiate ProcessClient
                var loCls = new R_ProcessAndUploadClient(
                    plSendWithContext: false,
                    plSendWithToken: false,
                    poProcessProgressStatus: this);

                //prepare Batch Parameter
                var loBatchPar = new R_BatchParameter
                {
                    COMPANY_ID = "RCD",
                    USER_ID = "cp",
                    ClassName = "BatchAndExcelBack.BulkInsertCls",
                    BigObject = _employeeList
                };

                await loCls.R_BatchProcess<List<EmployeeDTO>>(loBatchPar, _employeeList.Count);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        #region PROCESS STATUS

        Task R_IProcessProgressStatus.ProcessComplete(string pcKeyGuid, eProcessResultMode poProcessResultMode)
        {
            if (poProcessResultMode == eProcessResultMode.Success)
            {
                Message = string.Format("Process Complete and success with GUID {0}", pcKeyGuid);
                ShowSuccessAction();
            }

            if (poProcessResultMode == eProcessResultMode.Fail)
            {
                Message = string.Format("Process Complete but fail with GUID {0}", pcKeyGuid);

                //var loError = await ServiceGetError(pcKeyGuid);
                //ShowErrorAction(loError);
            }

            StateChangeAction();

            return Task.CompletedTask;
        }

        Task R_IProcessProgressStatus.ProcessError(string pcKeyGuid, R_APIException ex)
        {
            Message = string.Format("Process Error with GUID {0}", pcKeyGuid);

            ShowErrorAction(ex);
            StateChangeAction();

            return Task.CompletedTask;
        }

        Task R_IProcessProgressStatus.ReportProgress(int pnProgress, string pcStatus)
        {
            Message = string.Format("Process Progress {0} with status {1}", pnProgress, pcStatus);

            Percentage = pnProgress;
            Message = string.Format("Process Progress {0} with status {1}", pnProgress, pcStatus);

            StateChangeAction();

            return Task.CompletedTask;
        }

        #endregion
    }
}
