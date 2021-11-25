using DeveloperPath.WebUI.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using System.Globalization;
using System.Threading.Tasks;

namespace DeveloperPath.WebUI.Extensions
{
  public static class WebAssemblyHostExtension
  {
    public async static Task SetDefaultCulture(this WebAssemblyHost host)
    {
      var jsInterop = host.Services.GetRequiredService<IJSRuntime>();
      var result = await jsInterop.InvokeAsync<string>("cultureService.get");
      CultureInfo culture;
      if (result != null)
        culture = new CultureInfo(result);
      else
        culture = new CultureInfo("en-US");

      CultureInfo.DefaultThreadCurrentCulture = culture;
      CultureInfo.DefaultThreadCurrentUICulture = culture;
    }

    public async static Task SetCurrentTheme(this WebAssemblyHost host, AppState appState)
    {
      var jsInterop = host.Services.GetRequiredService<IJSRuntime>();
      var result = await jsInterop.InvokeAsync<string>("currentTheme.get");

      appState.SetTheme(result);
    }
  }
}
