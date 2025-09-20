using BatchAndExcelCommon.DTOs;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using System.Collections.ObjectModel;
using System.Data;

namespace BatchAndExcel.Pages.Excel
{
    public class ExcelViewModel
    {
        public ObservableCollection<EmployeeDTO> EmployeeList { get; private set; }

        public void SetEmployeeListFromDataSet(DataSet poDataSetEmployee)
        {
            var loEx = new R_Exception();

            try
            {
                var loResultEmployee = R_FrontUtility.R_ConvertTo<EmployeeDTO>(poDataSetEmployee.Tables["Employee"]);
                var loResultGender = R_FrontUtility.R_ConvertTo<GenderDTO>(poDataSetEmployee.Tables["Gender"]);

                foreach (var loEmployee in loResultEmployee)
                {
                    loEmployee.DHIRE_DATE = R_FrontUtility.R_ConvertToDateTime(loEmployee.HireDate, "yyyyMMdd");
                }

                EmployeeList = new ObservableCollection<EmployeeDTO>(loResultEmployee);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public DataTable CreateDataTableEmployee()
        {
            var loEx = new R_Exception();
            DataTable loDataTable = null;

            try
            {
                var loEmployeeList = new List<EmployeeDTO>()
                {
                    new EmployeeDTO() { Id = Guid.NewGuid().ToString(), FirstName = "Sihol", Gender = "M", HireDate = "20250917", WNI = false },
                    new EmployeeDTO() { Id = Guid.NewGuid().ToString(), FirstName = "Siholwati", Gender = "F", HireDate = "20250917" , WNI = true }
                };

                loDataTable = R_FrontUtility.R_ConvertTo(loEmployeeList);
                loDataTable.TableName = "Employee";
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loDataTable;
        }

        public DataTable CreateDataTableGender()
        {
            var loEx = new R_Exception();
            DataTable loDataTable = null;

            try
            {
                var loGenderList = new List<GenderDTO>()
                {
                    new GenderDTO { Id = "F", Desc = "Female" },
                    new GenderDTO { Id = "M", Desc = "Male" }
                };

                loDataTable = R_FrontUtility.R_ConvertTo(loGenderList);
                loDataTable.TableName = "Gender";
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loDataTable;
        }
    }

    public class GenderDTO
    {
        public string Id { get; set; }
        public string Desc { get; set; }
    }
}
