using BatchAndExcelCommon.DTOs;
using R_APIClient;
using R_BlazorFrontEnd.Exceptions;

namespace BatchAndExcel.Pages.Storage
{
    public class StorageViewModel
    {
        public string StorageId { get; set; }

        public byte[] ByteFile = Array.Empty<byte>();
        public string FileName = string.Empty;
        public string FileExtension = string.Empty;

        #region ADD ATTACHMENT

        public async Task AddAttachmentEmployeeAsync()
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = new AddAttachmentParameterDTO()
                {
                    CompanyId = "001",
                    EmployeeId = "Employee02",
                    FileName = FileName,
                    FileExtension = FileExtension,
                    Data = ByteFile,
                    UserId = "cp"
                };

                var lcStorageId = await AddAttachmentAsync(loParam);
                if (!string.IsNullOrWhiteSpace(lcStorageId))
                    StorageId = lcStorageId;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task<string> AddAttachmentAsync(AddAttachmentParameterDTO poParameter)
        {
            var loEx = new R_Exception();
            var lcResult = string.Empty;

            try
            {
                R_HTTPClientWrapper.httpClientName = "R_DefaultServiceUrl";
                var loResult = await R_HTTPClientWrapper.R_APIRequestObject<StorageResultDTO<string>, AddAttachmentParameterDTO>(
                    "api/Storage",
                    "AddAttachment",
                    poParameter,
                    "",
                    false,
                    false);

                if (loResult is not null)
                    lcResult = loResult.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return lcResult;
        }

        #endregion

        #region EDIT ATTACHMENT

        public async Task UpdateAttachmentEmployeeAsync()
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = new UpdateAttachmentParameterDTO()
                {
                    Data = ByteFile,
                    UserId = "cp",
                    StorageId = StorageId
                };

                var lcStorageId = await UpdateAttachmentAsync(loParam);
                if (!string.IsNullOrWhiteSpace(lcStorageId))
                    StorageId = lcStorageId;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task<string> UpdateAttachmentAsync(UpdateAttachmentParameterDTO poParameter)
        {
            var loEx = new R_Exception();
            var lcResult = string.Empty;

            try
            {
                R_HTTPClientWrapper.httpClientName = "R_DefaultServiceUrl";
                var loResult = await R_HTTPClientWrapper.R_APIRequestObject<StorageResultDTO<string>, UpdateAttachmentParameterDTO>(
                    "api/Storage",
                    "UpdateAttachment",
                    poParameter,
                    "",
                    false,
                    false);

                if (loResult is not null)
                    lcResult = loResult.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return lcResult;
        }

        #endregion

        #region DELETE ATTACHMENT

        public async Task DeleteAttachmentEmployeeAsync()
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = new DeleteAttachmentParameterDTO()
                {
                    UserId = "cp",
                    StorageId = StorageId
                };

                await DeleteAttachmentAsync(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task DeleteAttachmentAsync(DeleteAttachmentParameterDTO poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                R_HTTPClientWrapper.httpClientName = "R_DefaultServiceUrl";
                var loResult = await R_HTTPClientWrapper.R_APIRequestObject<StorageResultDTO, DeleteAttachmentParameterDTO>(
                    "api/Storage",
                    "DeleteAttachment",
                    poParameter,
                    "",
                    false,
                    false);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        #endregion

        #region GET ATTACHMENT

        public async Task<GetAttachmentDTO> GetAttachmentEmployeeAsync()
        {
            var loEx = new R_Exception();
            var loAttachment = new GetAttachmentDTO();

            try
            {
                var loParam = new GetAttachmentParameterDTO()
                {
                    StorageId = StorageId
                };

                loAttachment = await GetAttachmentAsync(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loAttachment;
        }

        private async Task<GetAttachmentDTO> GetAttachmentAsync(GetAttachmentParameterDTO poParameter)
        {
            var loEx = new R_Exception();
            var loResult = new GetAttachmentDTO();

            try
            {
                R_HTTPClientWrapper.httpClientName = "R_DefaultServiceUrl";
                var loRtn = await R_HTTPClientWrapper.R_APIRequestObject<StorageResultDTO<GetAttachmentDTO>, GetAttachmentParameterDTO>(
                    "api/Storage",
                    "GetAttachment",
                    poParameter,
                    "",
                    false,
                    false);

                if (loRtn is not null)
                    loResult = loRtn.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }

        #endregion
    }
}
