using BlazorTraining.Controls.Shared;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls.Popup;

namespace SAB03100Front.TextBox
{
    public partial class TextBoxMain
    {
        [Inject] private R_PopupService PopupService { get; set; }

        private async Task AllowSpacesOnClick()
        {
            await PopupService.Show(
                typeof(DemoPopup<TextBox_Demo_AllowSpaces>),
                null,
                poPopupSettings: new R_PopupSettings { PageTitle = "TextBox AllowSpaces" });
        }

        private async Task AllowSpecialCharactersOnClick()
        {
            await PopupService.Show(
                typeof(DemoPopup<TextBox_Demo_AllowSpecialCharacters>),
                null,
                poPopupSettings: new R_PopupSettings { PageTitle = "TextBox AllowSpecialCharacters" });
        }

        private async Task CharacterCasingOnClick()
        {
            await PopupService.Show(
                typeof(DemoPopup<TextBox_Demo_CharacterCasing>),
                null,
                poPopupSettings: new R_PopupSettings { PageTitle = "TextBox CharacterCasing" });
        }

        private async Task EnabledOnClick()
        {
            await PopupService.Show(
                typeof(DemoPopup<TextBox_Demo_Enabled>),
                null,
                poPopupSettings: new R_PopupSettings { PageTitle = "TextBox Enabled" });
        }

        private async Task MaxLengthOnClick()
        {
            await PopupService.Show(
                typeof(DemoPopup<TextBox_Demo_MaxLength>),
                null,
                poPopupSettings: new R_PopupSettings { PageTitle = "TextBox MaxLength" });
        }
    }
}
