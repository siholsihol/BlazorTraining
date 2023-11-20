using BatchAndExcelCommon.DTOs;
using R_Common;
using R_Storage;
using R_StorageCommon;

namespace BatchAndExcelBack
{
    public class StorageCls
    {
        private static List<StorageDTO> _storageData = new List<StorageDTO>()
        {
            new StorageDTO() { CompanyId = "001", EmployeeId = "Employee02", FileExtension = ".jpg", FileName="Day_old_chick.jpg", StorageId = "f91c49d99be34b03930b5ae08ed97e61"}
        };

        public EmployeeAttachmentDTO GetAttachment(EmployeeAttachmentDTO poParameter)
        {
            var loEx = new R_Exception();
            EmployeeAttachmentDTO loResult = default;

            try
            {
                var loStorage = _storageData.Where(x => x.CompanyId == poParameter.CompanyId &&
                                                    x.EmployeeId == poParameter.EmployeeId &&
                                                    x.FileName == poParameter.FileName).FirstOrDefault();

                if (loStorage != null || string.IsNullOrWhiteSpace(loStorage.StorageId))
                    throw new Exception("Data not found");

                var loReadParameter = new R_ReadParameter()
                {
                    StorageId = loStorage.StorageId
                };

                var loReadResult = R_StorageUtility.ReadFile(loReadParameter, "R_DefaultConnectionString");

                loResult = new EmployeeAttachmentDTO()
                {
                    CompanyId = loStorage.CompanyId,
                    EmployeeId = loStorage.EmployeeId,
                    FileName = loStorage.FileName,
                    FileExtension = loStorage.FileExtension,
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

        public void AddAttachment(EmployeeAttachmentDTO poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                var loStorage = _storageData.Where(x => x.CompanyId == poParameter.CompanyId &&
                                                    x.EmployeeId == poParameter.EmployeeId &&
                                                    x.FileName == poParameter.FileName).FirstOrDefault();

                if (loStorage != null)
                    throw new Exception("Data already exist.");

                var loAddParameter = new R_AddParameter()
                {
                    StorageType = R_EStorageType.Cloud,
                    ProviderCloudStorage = R_EProviderForCloudStorage.azure,
                    FileName = poParameter.FileName,
                    FileExtension = poParameter.FileExtension,
                    UploadData = poParameter.Data,
                    UserId = "cp",
                    BusinessKeyParameter = new R_BusinessKeyParameter()
                    {
                        CCOMPANY_ID = poParameter.CompanyId,
                        CDATA_TYPE = "TestStorage",
                        CKEY01 = poParameter.EmployeeId,
                        CKEY02 = poParameter.FileName
                    }
                };

                var loSaveResult = R_StorageUtility.AddFile(loAddParameter, "R_DefaultConnectionString");

                _storageData.Add(new StorageDTO()
                {
                    CompanyId = poParameter.CompanyId,
                    EmployeeId = poParameter.EmployeeId,
                    FileName = poParameter.FileName,
                    FileExtension = poParameter.FileExtension,
                    StorageId = loSaveResult.StorageId
                });
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public void UpdateAttachment(EmployeeAttachmentDTO poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                var loStorage = _storageData.Where(x => x.CompanyId == poParameter.CompanyId &&
                                                    x.EmployeeId == poParameter.EmployeeId &&
                                                    x.FileName == poParameter.FileName).FirstOrDefault();

                if (loStorage == null)
                    throw new Exception("Data not exist.");

                var loUpdateParameter = new R_UpdateParameter()
                {
                    StorageId = loStorage.StorageId,
                    UploadData = poParameter.Data,
                    UserId = "cp",
                    OptionalSaveAs = new R_UpdateParameter.OptionalSaveAsParameter()
                    {
                        FileName = poParameter.FileName,
                        FileExtension = poParameter.FileExtension
                    }
                };
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public void DeleteAttachment(EmployeeAttachmentDTO poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                var loStorage = _storageData.Where(x => x.CompanyId == poParameter.CompanyId &&
                                                    x.EmployeeId == poParameter.EmployeeId &&
                                                    x.FileName == poParameter.FileName).FirstOrDefault();

                if (loStorage == null)
                    throw new Exception("Data not exist.");

                var loDeleteParameter = new R_DeleteParameter()
                {
                    StorageId = loStorage.StorageId,
                    UserId = "cp"
                };

                R_StorageUtility.DeleteFile(loDeleteParameter, "R_DefaultConnectionString");

                _storageData.Remove(loStorage);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}
