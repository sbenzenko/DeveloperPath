using System.Globalization;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace WebUI.Blazor.Shared
{
    public partial class CultureSelector
    {
        CultureInfo[] cultures = new[]
        {
            new CultureInfo("en-US"),
            new CultureInfo("ru-RU"),
        };

        [CascadingParameter] public MatTheme Theme { get; set; }
        [Inject] public NavigationManager NavManager { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }

        CultureInfo Culture
        {
            get => CultureInfo.CurrentCulture;
            set
            {
                if (CultureInfo.CurrentCulture != value)
                {
                    var js = (IJSInProcessRuntime) JSRuntime;
                    js.InvokeVoid("cultureService.set", value.Name);
                    NavManager.NavigateTo(NavManager.Uri, forceLoad: true);
                }
            }
        }
    }
}