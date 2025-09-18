using BatchAndExcelCommon.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using System.Collections.ObjectModel;
using Telerik.Blazor;

namespace BatchAndExcel.Pages
{
    public partial class Excel
    {
        [CascadingParameter] private DialogFactory Dialog { get; set; }
        [Inject] private R_IExcel ExcelProvider { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }

        private R_eFileSelectAccept[] accepts = { R_eFileSelectAccept.Excel };
        private ObservableCollection<EmployeeDTO> _gridList = new();

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

                var loResultEmployee = R_FrontUtility.R_ConvertTo<EmployeeDTO>(loDataSet.Tables[0]);

                foreach (var loEmployee in loResultEmployee)
                {
                    loEmployee.DHIRE_DATE = R_FrontUtility.R_ConvertToDateTime(loEmployee.HireDate, "yyyyMMdd");
                }

                _gridList = new ObservableCollection<EmployeeDTO>(loResultEmployee);
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
                var loEmployeeList = new List<EmployeeDTO>()
            {
                new EmployeeDTO() {Id = Guid.NewGuid().ToString(), FirstName="Sihol", Gender="M", HireDate = "20250917"},
                new EmployeeDTO() {Id = Guid.NewGuid().ToString(), FirstName="Siholwati", Gender="F", HireDate = "20250917"}
            };

                var loDataTable = R_FrontUtility.R_ConvertTo(loEmployeeList);
                loDataTable.TableName = "Employee";

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
