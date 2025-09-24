using BatchAndExcelCommon.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using R_APIClient;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.Enums;
using R_BlazorFrontEnd.Exceptions;
using System.Data;
using Telerik.Blazor;

namespace BatchAndExcel.Pages.ExcelBatchProcess
{
    public partial class ExcelBatchProcess
    {
        [CascadingParameter] private DialogFactory Dialog { get; set; }
        [Inject] private R_IExcel ExcelProvider { get; set; }
        [Inject] private IJSRuntime JS { get; set; }

        private ExcelBatchProcessViewModel _excelBatchProcessViewModel = new();
        private R_eFileSelectAccept[] accepts = { R_eFileSelectAccept.Excel };
        //private long _maximumFileSize = 5 * 1024 * 1024;
        private DataSet _dataSetEmployee = new();

        protected override async Task OnInitializedAsync()
        {
            var loEx = new R_Exception();

            try
            {
                _excelBatchProcessViewModel.StateChangeAction = () => StateHasChanged();
                _excelBatchProcessViewModel.ShowErrorAction = async (DataSet poDataSet) =>
                {
                    await ShowErrorInvoke(poDataSet);
                };
                _excelBatchProcessViewModel.ShowSuccessAction = ShowSuccessInvoke;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            if (loEx.HasError)
                await Dialog.AlertAsync(loEx.ErrorList[0].ErrDescp, "Error");
        }

        #region DownloadFile

        private async Task OnClickDownloadFile()
        {
            var loEx = new R_Exception();

            try
            {
                R_HTTPClientWrapper.httpClientName = "R_DefaultServiceUrl";
                var loRtn = await R_HTTPClientWrapper.R_APIRequestObject<DownloadFileDTO>(
                        "api/file",
                        "DownloadFileExcelEmployee",
                        "",
                        false,
                        false);

                var lcSaveFileName = "TestExcelBatchProcess.xlsx";

                await JS.downloadFileFromStreamHandler(lcSaveFileName, loRtn.FileBytes);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            if (loEx.HasError)
                await Dialog.AlertAsync(loEx.ErrorList[0].ErrDescp, "Error");
        }

        #endregion

        private async Task OnChangeHandler(InputFileChangeEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                _excelBatchProcessViewModel.Message = "";

                //read excel as byte
                var loMS = new MemoryStream();
                await eventArgs.File.OpenReadStream(maxAllowedSize: _excelBatchProcessViewModel.MaximumFileSize).CopyToAsync(loMS);
                var loByteFile = loMS.ToArray();

                _dataSetEmployee.Tables.Clear();

                //import from excel
                _dataSetEmployee = ExcelProvider.R_ReadFromExcel(loByteFile);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            if (loEx.HasError)
                await Dialog.AlertAsync(loEx.ErrorList[0].ErrDescp, "Error");
        }

        private async Task OnClickHandler()
        {
            var loEx = new R_Exception();

            try
            {
                await _excelBatchProcessViewModel.SaveBatchEmployeeAsync(_dataSetEmployee);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            if (loEx.HasError)
                await Dialog.AlertAsync(loEx.ErrorList[0].ErrDescp, "Error");
        }

        private async Task OnClickHandlerWithResources()
        {
            var loEx = new R_Exception();

            try
            {
                await _excelBatchProcessViewModel.SaveBatchEmployeeWithResourcesAsync(_dataSetEmployee);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            if (loEx.HasError)
                await Dialog.AlertAsync(loEx.ErrorList[0].ErrDescp, "Error");
        }

        #region Handler

        public async Task ShowErrorInvoke(DataSet poDataSet)
        {
            //export to excel
            var loByteFile = ExcelProvider.R_WriteToExcel(poDataSet);
            var saveFileName = $"{Guid.NewGuid().ToString()}.xlsx";

            await JS.downloadFileFromStreamHandler(saveFileName, loByteFile);
        }

        public void ShowSuccessInvoke()
        {
            //TO DO Success upload
        }

        #endregion
    }
}
