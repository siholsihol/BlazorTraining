using BatchAndExcelCommon.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.Enums;
using R_BlazorFrontEnd.Excel.Mappings;
using R_BlazorFrontEnd.Exceptions;
using System.Data;
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
                var loMapping = new List<R_DataColumnMapping>
                {
                    new R_DataColumnMapping(0,"Id"),
                    new R_DataColumnMapping(1,nameof(EmployeeDTO.FirstName)),
                    new R_DataColumnMapping(2,"Gender"),
                    new R_DataColumnMapping(3,"HireDate"),
                    new R_DataColumnMapping(4,"WNI"),
                };

                //var loDataSet = ExcelProvider.R_ReadExcel(loByteFile);
                var loDataSet = ExcelProvider.R_ReadExcel(loByteFile, action =>
                {
                    action.WithHeader = true;
                    action.TableNames = new string[] { "Employee", "Gender" };
                    //action.ColumnMappings = loMapping;
                    //action.Range = "A2:E2";
                });

                _excelViewModel.SetEmployeeListFromDataSet(loDataSet);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            if (loEx.HasError)
                await Dialog.AlertAsync(loEx.ErrorList[0].ErrDescp, "Error");
        }

        private void SetParameterExcel(R_ReadFromExcelOption option)
        {
            var loMapping = new List<R_DataColumnMapping>
                {
                    new R_DataColumnMapping(0,"Id"),
                    new R_DataColumnMapping(1,nameof(EmployeeDTO.FirstName)),
                    new R_DataColumnMapping(2,"Gender"),
                    new R_DataColumnMapping(3,"HireDate"),
                    new R_DataColumnMapping(4,"WNI"),
                };

            option.WithHeader = true;
            option.TableNames = new string[] { "Employee" };
            option.ColumnMappings = loMapping;
            option.Range = "A2:E2";
        }

        private async Task OnClickHandler()
        {
            var loEx = new R_Exception();

            try
            {
                var loDataTableEmployee = _excelViewModel.CreateDataTableEmployee();
                if (loDataTableEmployee is null)
                    return;

                var loDataTableGender = _excelViewModel.CreateDataTableGender();
                if (loDataTableGender is null)
                    return;

                //export to excel
                //var loByteFile = ExcelProvider.R_WriteToExcel(loDataTableEmployee);
                var loDataSet = new DataSet();
                loDataSet.Tables.Add(loDataTableEmployee);
                loDataSet.Tables.Add(loDataTableGender);

                var loByteFile = ExcelProvider.R_WriteToExcel(loDataSet);

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
