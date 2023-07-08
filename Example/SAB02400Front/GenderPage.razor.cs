using DataDummyProvider.DTOs;
using DataDummyProvider.Services;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Exceptions;
using System.Collections.ObjectModel;

namespace SAB02400Front
{
    public partial class GenderPage : R_Page
    {
        private R_Grid<GenderDTO> _gridRef;
        public ObservableCollection<GenderDTO> GenderList { get; set; } = new ObservableCollection<GenderDTO>();

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                await _gridRef.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task OnClose()
        {
            await Close(true, _gridRef.GetCurrentData());
        }

        private void R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loGenderList = CustomerService.GetGenders();
                GenderList = new ObservableCollection<GenderDTO>(loGenderList);

                eventArgs.ListEntityResult = GenderList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}
