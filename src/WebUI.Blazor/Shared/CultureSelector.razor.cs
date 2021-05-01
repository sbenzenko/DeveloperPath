using System.Globalization;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace WebUI.Blazor.Shared
{
    public partial class CultureSelector
    {
        [Inject] public IJSRuntime JSRuntime { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
    }
}