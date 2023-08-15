using BatchAndExcelBack;
using BatchAndExcelCommon.DTOs;
using Microsoft.AspNetCore.Mvc;
using R_Common;

namespace BatchAndExcelService
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StorageController : ControllerBase
    {
        [HttpPost]
        public EmployeeAttachmentDTO GetAttachment(EmployeeAttachmentDTO poParameter)
        {
            var loEx = new R_Exception();
            EmployeeAttachmentDTO loRtn = default;

            try
            {
                var loCls = new StorageCls();
                loRtn = loCls.GetAttachment(poParameter);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public StorageResultDTO AddAttachment(EmployeeAttachmentDTO poParameter)
        {
            var loEx = new R_Exception();
            StorageResultDTO loResult = new();

            try
            {
                var loCls = new StorageCls();
                loCls.AddAttachment(poParameter);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }

        [HttpPost]
        public void UpdateAttachment(EmployeeAttachmentDTO poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                var loCls = new StorageCls();
                loCls.UpdateAttachment(poParameter);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        [HttpPost]
        public void DeleteAttachment(EmployeeAttachmentDTO poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                var loCls = new StorageCls();
                loCls.DeleteAttachment(poParameter);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}
