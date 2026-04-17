using Microsoft.AspNetCore.Components;
using R_APICommonDTO;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using Telerik.Blazor;

namespace BatchAndExcel.Pages.Batch
{
    public partial class Batch
    {
        [CascadingParameter] private DialogFactory Dialog { get; set; }

        private BatchViewModel _batchViewModel = new();

        protected override async Task OnInitializedAsync()
        {
            var loEx = new R_Exception();

            try
            {
                _batchViewModel.StateChangeAction = () => StateHasChanged();
                _batchViewModel.ShowErrorAction = async (R_APIException exception) =>
                {
                    await ShowErrorInvoke(exception);
                };
                _batchViewModel.ShowSuccessAction = ShowSuccessInvoke;

                _batchViewModel.GenerateEmployeeData();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            if (loEx.HasError)
                await Dialog.AlertAsync(loEx.ErrorList[0].ErrDescp, "Error");
        }

        #region Batch

        public async Task OnClickHandler()
        {
            var loEx = new R_Exception();

            try
            {
                await _batchViewModel.BatchProcessEmployeeAsync();
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
