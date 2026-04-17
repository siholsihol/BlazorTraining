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
        public StorageResultDTO<GetAttachmentDTO> GetAttachment(GetAttachmentParameterDTO poParameter)
        {
            var loEx = new R_Exception();
            StorageResultDTO<GetAttachmentDTO> loRtn = new();

            try
            {
                var loCls = new StorageCls();
                var loResult = loCls.GetAttachment(poParameter.StorageId);

                loRtn.Data = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public StorageResultDTO<string> AddAttachment(AddAttachmentParameterDTO poParameter)
        {
            var loEx = new R_Exception();
            StorageResultDTO<string> loResult = new();

            try
            {
                var loCls = new StorageCls();
                var lcStorageId = loCls.AddAttachment(poParameter);

                loResult = new StorageResultDTO<string>
                {
                    Data = lcStorageId
                };
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }

        [HttpPost]
        public StorageResultDTO<string> UpdateAttachment(UpdateAttachmentParameterDTO poParameter)
        {
            var loEx = new R_Exception();
            StorageResultDTO<string> loResult = new();

            try
            {
                var loCls = new StorageCls();
                var lcStorageId = loCls.UpdateAttachment(poParameter);

                loResult = new StorageResultDTO<string>
                {
                    Data = lcStorageId
                };
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }

        [HttpPost]
        public StorageResultDTO DeleteAttachment(DeleteAttachmentParameterDTO poParameter)
        {
            var loEx = new R_Exception();
            var loResult = new StorageResultDTO();

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

            return loResult;
        }
    }
}
