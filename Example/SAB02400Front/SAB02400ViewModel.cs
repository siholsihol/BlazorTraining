using DataDummyProvider.DTOs;
using DataDummyProvider.Services;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Excel;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using System.Collections.ObjectModel;

namespace SAB02400Front
{
    public class SAB02400ViewModel : R_ViewModel<UserDTO>
    {
        public ObservableCollection<UserDTO> UserList { get; set; } = new ObservableCollection<UserDTO>();
        public List<GenderDTO> GenderList = new List<GenderDTO>();

        #region Excel
        public void ReadExcel(byte[] poExcelByte)
        {
            var loEx = new R_Exception();

            try
            {
                var loExcel = new R_Excel();
                var loDataSet = loExcel.R_ReadFromExcel(poExcelByte);

                var loResult = R_FrontUtility.R_ConvertTo<UserDTO>(loDataSet.Tables[0]);

                UserList = new ObservableCollection<UserDTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public byte[] WriteExcel(List<UserDTO> poListData)
        {
            var loEx = new R_Exception();
            byte[] loResult = null;

            try
            {
                var loDataTable = R_FrontUtility.R_ConvertTo(poListData);

                var loExcel = new R_Excel();
                loResult = loExcel.R_WriteToExcel(loDataTable);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
        #endregion

        public void GetUserList()
        {
            var loEx = new R_Exception();

            try
            {
                var loUserList = UserService.GetUsers();

                UserList = new ObservableCollection<UserDTO>(loUserList);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public void GetGenderList()
        {
            var loEx = new R_Exception();

            try
            {
                GenderList = CustomerService.GetGenders();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}
