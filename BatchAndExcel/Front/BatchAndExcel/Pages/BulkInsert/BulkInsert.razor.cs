using Microsoft.AspNetCore.Components;
using R_APICommonDTO;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using Telerik.Blazor;

namespace BatchAndExcel.Pages.BulkInsert
{
    public partial class BulkInsert
    {
        [CascadingParameter] private DialogFactory Dialog { get; set; }

        private BulkInsertViewModel _bulkInsertViewModel = new();

        protected override void OnInitialized()
        {
            var loEx = new R_Exception();

            try
            {
                _bulkInsertViewModel.StateChangeAction = () => StateHasChanged();
                _bulkInsertViewModel.ShowErrorAction = async (R_APIException exception) =>
                {
                    await ShowErrorInvoke(exception);
                };
                _bulkInsertViewModel.ShowSuccessAction = ShowSuccessInvoke;

                _bulkInsertViewModel.GenerateEmployeeData();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task OnClickHandler()
        {
            var loEx = new R_Exception();

            try
            {
                await _bulkInsertViewModel.BulkInsertProcessEmployee();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        #region Handler

        public async Task ShowErrorInvoke(R_APIException poException)
        {
            var loEx = R_FrontUtility.R_ConvertFromAPIException(poException);
            //this.R_DisplayException(loEx);

            if (loEx.HasError)
                await Dialog.AlertAsync(string.Join(" | ", loEx.ErrorList.Select(x => x.ErrDescp)), "Error");
        }

        public void ShowSuccessInvoke()
        {
            //TO DO Success upload
        }

        #endregion
    }
}
