using BatchAndExcelCommon.DTOs;
using Microsoft.AspNetCore.Components.Forms;
using R_APIClient;
using R_BlazorFrontEnd.Controls.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;

namespace BatchAndExcel.Pages
{
    public partial class Storage
    {
        private R_eFileSelectAccept[] accepts = { R_eFileSelectAccept.Image };
        private byte[] _byteFile = default;
        private string _fileName = default;
        private string _fileExtension = default;

        private async Task OnChangeHandler(InputFileChangeEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                _fileName = eventArgs.File.Name;
                _fileExtension = Path.GetExtension(_fileName);

                //read excel as byte
                var loMS = new MemoryStream();
                await eventArgs.File.OpenReadStream().CopyToAsync(loMS);
                _byteFile = loMS.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(R_FrontUtility.Dump(ex));
            }
        }

        #region Add
        private async Task OnClickHandlerAdd()
        {
            try
            {
                var loParam = new EmployeeAttachmentDTO()
                {
                    CompanyId = "001",
                    EmployeeId = "Employee02",
                    FileName = _fileName,
                    FileExtension = _fileExtension,
                    Data = _byteFile
                };

                await AddAttachmentAsync(loParam);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task AddAttachmentAsync(EmployeeAttachmentDTO poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                R_HTTPClientWrapper.httpClientName = "R_DefaultServiceUrl";
                var loResult = await R_HTTPClientWrapper.R_APIRequestObject<StorageResultDTO, EmployeeAttachmentDTO>(
                    "api/Storage",
                    "AddAttachment",
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

        #region Get
        private async Task OnClickHandlerGet()
        {
            try
            {
                var loParam = new EmployeeAttachmentDTO()
                {
                    CompanyId = "001",
                    EmployeeId = "Employee02",
                    FileName = _fileName
                };

                await GetAttachmentAsync(loParam);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task GetAttachmentAsync(EmployeeAttachmentDTO poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                R_HTTPClientWrapper.httpClientName = "R_DefaultServiceUrl";
                var loResult = await R_HTTPClientWrapper.R_APIRequestObject<StorageResultDTO, EmployeeAttachmentDTO>(
                    "api/Storage",
                    "GetAttachment",
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
    }
}
