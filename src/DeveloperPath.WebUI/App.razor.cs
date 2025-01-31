using System.Threading.Tasks;

using DeveloperPath.WebUI.Resources;
using DeveloperPath.WebUI.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace DeveloperPath.WebUI;

public partial class App
{
  [Inject] public IStringLocalizer<LanguageResources> Localizer { get; set; }
  [Inject] public AppState AppState { get; set; }

  protected override Task OnInitializedAsync()
  {
    AppState.OnChange += AppState_OnChange;
    return base.OnInitializedAsync();
  }

  private void AppState_OnChange()
  {
    StateHasChanged();
  }
}