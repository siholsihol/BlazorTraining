using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.Enums;
using R_BlazorFrontEnd.Exceptions;
using Telerik.Blazor;

namespace BatchAndExcel.Pages.Excel
{
    public partial class Excel
    {
        [CascadingParameter] private DialogFactory Dialog { get; set; }
        [Inject] private R_IExcel ExcelProvider { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }

        private ExcelViewModel _excelViewModel = new();
        private R_eFileSelectAccept[] _accepts = { R_eFileSelectAccept.Excel };
        //private ObservableCollection<EmployeeDTO> _gridList = new();

        #region FILE SELECT

        private async Task OnChangeHandler(InputFileChangeEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                //read excel as byte
                var loMS = new MemoryStream();
                await eventArgs.File.OpenReadStream().CopyToAsync(loMS);
                var loByteFile = loMS.ToArray();

                //import from excel
                var loDataSet = ExcelProvider.R_ReadFromExcel(loByteFile, new string[] { "Employee" });

                _excelViewModel.SetEmployeeListFromDataSet(loDataSet);
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
                var loDataTable = _excelViewModel.CreateDataTableEmployee();
                if (loDataTable is null)
                    return;

                //export to excel
                var loByteFile = ExcelProvider.R_WriteToExcel(loDataTable);
                var lcSaveFileName = $"{Guid.NewGuid().ToString()}.xlsx";

                await JSRuntime.downloadFileFromStreamHandler(lcSaveFileName, loByteFile);
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
