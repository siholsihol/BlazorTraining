using BatchAndExcelCommon.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using R_APIClient;
using R_APICommonDTO;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;
using R_ProcessAndUploadFront;

namespace BatchAndExcel.Pages
{
    public partial class File
    {
        [Inject] private IJSRuntime JSRuntime { get; set; }

        private UploadFileViewModel _uploadFileViewModel = new();
        //private int _percentage = 0;
        //private string _message = string.Empty;
        private R_eFileSelectAccept[] accepts = { R_eFileSelectAccept.Doc };
        private byte[] fileByte = null;

        //public void StateChangeInvoke()
        //{
        //    StateHasChanged();
        //}

        public void ShowErrorInvoke(R_APIException poException)
        {
            var loEx = R_FrontUtility.R_ConvertFromAPIException(poException);
            //this.R_DisplayException(loEx);
            Console.WriteLine(R_FrontUtility.Dump(loEx));
        }

        public void ShowSuccessInvoke()
        {
            //TO DO Success upload
        }

        protected override void OnInitialized()
        {
            _uploadFileViewModel.StateChangeAction = () => StateHasChanged();
            _uploadFileViewModel.ShowErrorAction = ShowErrorInvoke;
            _uploadFileViewModel.ShowSuccessAction = ShowSuccessInvoke;
        }

        #region UploadFile

        private async Task OnChangeHandler(InputFileChangeEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                //read file as byte
                var loMS = new MemoryStream();
                await eventArgs.File.OpenReadStream().CopyToAsync(loMS);
                fileByte = loMS.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(R_FrontUtility.Dump(ex));
            }
        }

        private async Task OnClickAttachHandler()
        {
            var loEx = new R_Exception();
            R_UploadPar loUploadPar;
            R_ProcessAndUploadClient loCls;

            try
            {
                if (fileByte == null)
                    return;

                //Instantiate ProcessClient
                loCls = new R_ProcessAndUploadClient(
                    plSendWithContext: false,
                    plSendWithToken: false,
                    poProcessProgressStatus: _uploadFileViewModel);

                //add filebyte to DTO
                var loUploadFile = new UploadFileDTO { FileBytes = fileByte };

                //preapare Batch Parameter
                loUploadPar = new R_UploadPar
                {
                    COMPANY_ID = "RCD",
                    USER_ID = "cp",
                    ClassName = "BatchAndExcelBack.AttachFileCls",
                    BigObject = loUploadFile,
                    UserParameters = new List<R_KeyValue>
                    {
                        new R_KeyValue { Key = "key1", Value = "value1"}
                    }
                };

                await loCls.R_AttachFile<UploadFileDTO>(loUploadPar);
            }
            catch (Exception ex)
            {
                Console.WriteLine(R_FrontUtility.Dump(ex));
            }
        }

        #endregion

        #region DownloadFile

        private async Task OnClickDownloadFile()
        {
            try
            {
                R_HTTPClientWrapper.httpClientName = "R_DefaultServiceUrl";
                var loRtn = await R_HTTPClientWrapper.R_APIRequestObject<DownloadFileDTO>(
                        "api/file",
                        "DownloadFile",
                        "",
                        false,
                        false);

                var saveFileName = $"{Guid.NewGuid().ToString()}.docx";

                await JSRuntime.downloadFileFromStreamHandler(saveFileName, loRtn.FileBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion
    }
}
