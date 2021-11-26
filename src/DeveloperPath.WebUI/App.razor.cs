using DeveloperPath.WebUI.Resources;
using DeveloperPath.WebUI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace DeveloperPath.WebUI
{
    public partial class App
    {
        [Inject] public IStringLocalizer<LanguageResources> localizer { get; set; }
        [Inject] public AppState appState { get; set; }

        protected override Task OnInitializedAsync()
        {
            appState.OnChange += AppState_OnChange;
            return base.OnInitializedAsync();
        }

        private void AppState_OnChange()
        {
            StateHasChanged();
        }
    }
}
