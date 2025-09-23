using BlazorTraining.Controls.Shared;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls.Popup;
using SAB03100Front.TextBox.Events;
using SAB03100Front.TextBox.Methods;
using SAB03100Front.TextBox.Properties;

namespace SAB03100Front.TextBox
{
    public partial class TextBoxMain
    {
        [Inject] private R_PopupService PopupService { get; set; }

        #region PROPERTIES

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

        private async Task PasswordOnClick()
        {
            await PopupService.Show(
                typeof(DemoPopup<TextBox_Demo_Password>),
                null,
                poPopupSettings: new R_PopupSettings { PageTitle = "TextBox Password" });
        }

        private async Task PlaceholderOnClick()
        {
            await PopupService.Show(
                typeof(DemoPopup<TextBox_Demo_Placeholder>),
                null,
                poPopupSettings: new R_PopupSettings { PageTitle = "TextBox Placeholder" });
        }

        private async Task ReadOnlyOnClick()
        {
            await PopupService.Show(
                typeof(DemoPopup<TextBox_Demo_ReadOnly>),
                null,
                poPopupSettings: new R_PopupSettings { PageTitle = "TextBox ReadOnly" });
        }

        private async Task TabIndexOnClick()
        {
            await PopupService.Show(
                typeof(DemoPopup<TextBox_Demo_TabIndex>),
                null,
                poPopupSettings: new R_PopupSettings { PageTitle = "TextBox TabIndex" });
        }

        private async Task TextAlignmentOnClick()
        {
            await PopupService.Show(
                typeof(DemoPopup<TextBox_Demo_TextAlignment>),
                null,
                poPopupSettings: new R_PopupSettings { PageTitle = "TextBox TextAlignment" });
        }

        private async Task TooltipOnClick()
        {
            await PopupService.Show(
                typeof(DemoPopup<TextBox_Demo_Tooltip>),
                null,
                poPopupSettings: new R_PopupSettings { PageTitle = "TextBox Tooltip" });
        }

        #endregion

        #region EVENTS

        private async Task OnChangedOnClick()
        {
            await PopupService.Show(
                typeof(DemoPopup<TextBox_Demo_OnChanged>),
                null,
                poPopupSettings: new R_PopupSettings { PageTitle = "TextBox OnChanged" });
        }

        private async Task OnLostFocusOnClick()
        {
            await PopupService.Show(
                typeof(DemoPopup<TextBox_Demo_OnLostFocus>),
                null,
                poPopupSettings: new R_PopupSettings { PageTitle = "TextBox OnLostFocus" });
        }

        private async Task ValueChangedOnClick()
        {
            await PopupService.Show(
                typeof(DemoPopup<TextBox_Demo_ValueChanged>),
                null,
                poPopupSettings: new R_PopupSettings { PageTitle = "TextBox ValueChanged" });
        }

        #endregion

        #region METHODS

        private async Task FocusAsyncOnClick()
        {
            await PopupService.Show(
                typeof(DemoPopup<TextBox_Demo_FocusAsync>),
                null,
                poPopupSettings: new R_PopupSettings { PageTitle = "TextBox FocusAsync" });
        }

        #endregion
    }
}
