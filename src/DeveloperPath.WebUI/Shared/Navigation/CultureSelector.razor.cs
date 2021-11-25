using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DeveloperPath.WebUI.Shared.Navigation;

public partial class CultureSelector
{
  [Inject] public IJSRuntime JSRuntime { get; set; }
  [Inject] NavigationManager NavigationManager { get; set; }
}
