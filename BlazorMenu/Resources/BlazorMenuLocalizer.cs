using R_BlazorFrontEnd.Helpers;
using Telerik.Blazor.Services;

namespace BlazorMenu.Resources
{
    public class BlazorMenuLocalizer : ITelerikStringLocalizer
    {
        public string this[string name]
        {
            get
            {
                return GetStringFromResource(name);
            }
        }

        private string GetStringFromResource(string key)
        {
            var lcMessage = R_FrontUtility.R_GetMessage(typeof(BlazorMenuLocalizer), key, pcResourceName: "BlazorMenuResources");

            return lcMessage;
        }
    }
}
