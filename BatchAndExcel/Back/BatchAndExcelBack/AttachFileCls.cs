using BatchAndExcelCommon.DTOs;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;

namespace BatchAndExcelBack
{
    public class AttachFileCls : R_IAttachFileAsync
    {
        public Task R_AttachFileAsync(R_AttachFilePar poAttachFile)
        {
            var loEx = new R_Exception();

            try
            {
                var loFile = R_NetCoreUtility.R_DeserializeObjectFromByte<UploadFileDTO>(poAttachFile.BigObject);
                var lcCompanyId = poAttachFile.Key.COMPANY_ID;
                var lcUserId = poAttachFile.Key.USER_ID;
                var lcKeyGuid = poAttachFile.Key.KEY_GUID;
                var loUserParameters = poAttachFile.UserParameters;

                //TODO save file 
                var lcFileName = Path.Combine(@"D:\", Guid.NewGuid().ToString() + ".docx");

                R_NetCoreUtility.R_DeserializeFileFromByte(lcFileName, loFile.FileBytes);

                //simulate error
                //throw new Exception("eh ada error.");
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return Task.CompletedTask;
        }
    }
}
