using BatchAndExcelCommon.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.Enums;
using R_BlazorFrontEnd.Exceptions;
using Telerik.Blazor;

namespace BatchAndExcel.Pages.Storage
{
    public partial class Storage
    {
        [CascadingParameter] private DialogFactory Dialog { get; set; }

        [Inject] private IJSRuntime JS { get; set; }

        private R_eFileSelectAccept[] _accepts = { R_eFileSelectAccept.Image };
        private StorageViewModel _storageViewModel = new();

        private async Task OnChangeHandler(InputFileChangeEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                _storageViewModel.FileName = eventArgs.File.Name;
                _storageViewModel.FileExtension = Path.GetExtension(_storageViewModel.FileName);

                //read excel as byte
                var loMS = new MemoryStream();
                await eventArgs.File.OpenReadStream().CopyToAsync(loMS);
                _storageViewModel.ByteFile = loMS.ToArray();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            if (loEx.HasError)
                await Dialog.AlertAsync(loEx.ErrorList[0].ErrDescp, "Error");
        }

        #region Add

        private async Task OnClickHandlerAdd()
        {
            var loEx = new R_Exception();

            try
            {
                await _storageViewModel.AddAttachmentEmployeeAsync();

                await Dialog.ConfirmAsync("Success add to storage.");
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            if (loEx.HasError)
                await Dialog.AlertAsync(loEx.ErrorList[0].ErrDescp, "Error");
        }

        #endregion

        #region Update

        private async Task OnClickHandlerUpdate()
        {
            var loEx = new R_Exception();

            try
            {
                await _storageViewModel.UpdateAttachmentEmployeeAsync();

                await Dialog.ConfirmAsync("Success update to storage.");
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            if (loEx.HasError)
                await Dialog.AlertAsync(loEx.ErrorList[0].ErrDescp, "Error");
        }

        #endregion

        #region Delete

        private async Task OnClickHandlerDelete()
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = new DeleteAttachmentParameterDTO()
                {
                    UserId = "cp",
                    StorageId = _storageViewModel.StorageId
                };

                await _storageViewModel.DeleteAttachmentEmployeeAsync();

                await Dialog.ConfirmAsync("Success delete image.");
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            if (loEx.HasError)
                await Dialog.AlertAsync(loEx.ErrorList[0].ErrDescp, "Error");
        }

        #endregion

        #region Get

        private async Task OnClickHandlerGet()
        {
            var loEx = new R_Exception();

            try
            {
                var loAttachment = await _storageViewModel.GetAttachmentEmployeeAsync();

                if (loAttachment is not null)
                {
                    await JS.downloadFileFromStreamHandler(loAttachment.FileName, loAttachment.Data);
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            if (loEx.HasError)
                await Dialog.AlertAsync(loEx.ErrorList[0].ErrDescp, "Error");
        }

        #endregion
    }
}
