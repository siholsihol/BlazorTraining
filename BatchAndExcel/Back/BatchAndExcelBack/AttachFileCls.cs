﻿using BatchAndExcelCommon.DTOs;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;

namespace BatchAndExcelBack
{
    public class AttachFileCls : R_IAttachFile
    {
        public void R_AttachFile(R_AttachFilePar poAttachFile)
        {
            var loEx = new R_Exception();

            try
            {
                var loFile = R_NetCoreUtility.R_DeserializeObjectFromByte<UploadFileDTO>(poAttachFile.BigObject);

                //TODO save file 
                var lcFileName = Path.Combine(@"D:\", Guid.NewGuid().ToString() + ".docx");

                R_NetCoreUtility.R_DeserializeFileFromByte(lcFileName, loFile.FileBytes);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}
