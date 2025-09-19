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
using Telerik.Blazor;

namespace BatchAndExcel.Pages.File
{
    public partial class File
    {
        [CascadingParameter] private DialogFactory Dialog { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }

        private FileViewModel _uploadFileViewModel = new();
        private R_eFileSelectAccept[] _accepts = { R_eFileSelectAccept.Doc };
        private byte[] _fileByte = null;

        //public void StateChangeInvoke()
        //{
        //    StateHasChanged();
        //}

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
                _fileByte = loMS.ToArray();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            if (loEx.HasError)
                await Dialog.AlertAsync(loEx.ErrorList[0].ErrDescp, "Error");
        }

        private async Task OnClickAttachHandler()
        {
            var loEx = new R_Exception();

            try
            {
                if (_fileByte == null)
                    return;

                //Instantiate ProcessClient
                var loCls = new R_ProcessAndUploadClient(
                    plSendWithContext: false,
                    plSendWithToken: false,
                    poProcessProgressStatus: _uploadFileViewModel);

                //add filebyte to DTO
                var loUploadFile = new UploadFileDTO { FileBytes = _fileByte };

                //preapare Batch Parameter
                var loUploadPar = new R_UploadPar
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
                loEx.Add(ex);
            }

            if (loEx.HasError)
                await Dialog.AlertAsync(loEx.ErrorList[0].ErrDescp, "Error");
        }

        #endregion

        #region DownloadFile

        private async Task OnClickDownloadFile()
        {
            var loEx = new R_Exception();

            try
            {
                R_HTTPClientWrapper.httpClientName = "R_DefaultServiceUrl";
                var loRtn = await R_HTTPClientWrapper.R_APIRequestObject<DownloadFileDTO>(
                        "api/file",
                        "DownloadFile",
                        "",
                        false,
                        false);

                var lcSaveFileName = $"{Guid.NewGuid().ToString()}.docx";

                await JSRuntime.downloadFileFromStreamHandler(lcSaveFileName, loRtn.FileBytes);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            if (loEx.HasError)
                await Dialog.AlertAsync(loEx.ErrorList[0].ErrDescp, "Error");
        }

        #endregion

        #region Handler

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

        #endregion
    }
}
