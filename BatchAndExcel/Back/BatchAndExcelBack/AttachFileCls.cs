using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;

namespace BatchAndExcelBack
{
    public class FileDTO
    {
        public byte[] Data { get; set; }
    }

    public class AttachFileCls : R_IAttachFile
    {
        public void R_AttachFile(R_AttachFilePar poAttachFile)
        {
            var loEx = new R_Exception();
            byte[] loByte = null;

            try
            {
                var loDb = new R_Db();
                var lcCmd = $"SELECT dbo.RFN_CombineByte('{poAttachFile.Key.COMPANY_ID}','{poAttachFile.Key.USER_ID}','{poAttachFile.Key.KEY_GUID}_1') AS Data";
                var loFile = loDb.SqlExecObjectQuery<FileDTO>(lcCmd).FirstOrDefault();

                loByte = loFile.Data;
                var lcFileName = Path.Combine(@"D:\", Guid.NewGuid().ToString() + ".docx");

                R_NetCoreUtility.R_DeserializeFileFromByte(lcFileName, loByte);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}
