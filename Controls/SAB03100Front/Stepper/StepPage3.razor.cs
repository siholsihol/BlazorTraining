using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;

namespace SAB03100Front.Stepper
{
    public partial class StepPage3 : R_Page
    {
        private R_Conductor _conductorRef = null;
        private SAB03100ViewModel _viewModel = new();

        protected override async Task R_Init_From_Master(object Parameter)
        {
            _viewModel = (SAB03100ViewModel)Parameter;

            await Task.CompletedTask;
        }
    }
}
