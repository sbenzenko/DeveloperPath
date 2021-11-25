using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DeveloperPath.WebUI.Shared.Navigation
{
  public partial class ThemeSelector
  {
    [Inject] public IJSRuntime JSRuntime { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }
  }
}