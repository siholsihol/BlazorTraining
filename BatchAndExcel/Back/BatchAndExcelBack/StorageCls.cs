using BatchAndExcelCommon.DTOs;
using R_Common;
using R_Storage;
using R_StorageCommon;

namespace BatchAndExcelBack
{
    public class StorageCls
    {
        public GetAttachmentDTO GetAttachment(string pcStorageId)
        {
            var loEx = new R_Exception();
            GetAttachmentDTO loResult = new();

            try
            {
                var loReadParameter = new R_ReadParameter()
                {
                    StorageId = pcStorageId
                };

                var loReadResult = R_StorageUtility.ReadFile(loReadParameter, "R_DefaultConnectionString");

                loResult = new GetAttachmentDTO()
                {
                    FileName = loReadResult.FileName,
                    FileExtension = loReadResult.FileExtension,
                    Url = loReadResult.Url,
                    Data = loReadResult.Data
                };
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }

        public string AddAttachment(AddAttachmentParameterDTO poParameter)
        {
            var loEx = new R_Exception();
            var lcResult = string.Empty;

            try
            {
                var loAddParameter = new R_AddParameter()
                {
                    StorageType = R_EStorageType.Cloud,
                    ProviderCloudStorage = R_EProviderForCloudStorage.azure,
                    FileName = poParameter.FileName,
                    FileExtension = poParameter.FileExtension,
                    UploadData = poParameter.Data,
                    UserId = poParameter.UserId,
                    BusinessKeyParameter = new R_BusinessKeyParameter()
                    {
                        CCOMPANY_ID = poParameter.CompanyId,
                        CDATA_TYPE = "TestStorage",
                        CKEY01 = poParameter.EmployeeId,
                        CKEY02 = poParameter.FileName
                    }
                };

                var loSaveResult = R_StorageUtility.AddFile(loAddParameter, "R_DefaultConnectionString");

                if (loSaveResult is not null)
                    lcResult = loSaveResult.StorageId;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return lcResult;
        }

        public string UpdateAttachment(UpdateAttachmentParameterDTO poParameter)
        {
            var loEx = new R_Exception();
            var lcResult = string.Empty;

            try
            {
                var loUpdateParameter = new R_UpdateParameter()
                {
                    StorageId = poParameter.StorageId,
                    UploadData = poParameter.Data,
                    UserId = poParameter.UserId
                };

                var loUpdateResult = R_StorageUtility.UpdateFile(loUpdateParameter, "R_DefaultConnectionString");

                if (loUpdateResult is not null)
                    lcResult = loUpdateResult.StorageId;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return lcResult;
        }

        public void DeleteAttachment(DeleteAttachmentParameterDTO poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                var loDeleteParameter = new R_DeleteParameter()
                {
                    StorageId = poParameter.StorageId,
                    UserId = poParameter.UserId
                };

                R_StorageUtility.DeleteFile(loDeleteParameter, "R_DefaultConnectionString");
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}
