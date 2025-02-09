using System;
using System.Net.Http;
using System.Threading.Tasks;

using DeveloperPath.WebUI.Extensions;
using DeveloperPath.WebUI.Security;
using DeveloperPath.WebUI.Services;
using DeveloperPath.WebUI.Services.Administration;
using DeveloperPath.WebUI.UIHelpers;

using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MudBlazor;
using MudBlazor.Services;

namespace DeveloperPath.WebUI;

public class Program
{
  public static async Task Main(string[] args)
  {
    var builder = WebAssemblyHostBuilder.CreateDefault(args);
    builder.RootComponents.Add<App>("#app");

    // We register a named HttpClient here for the API
    Console.WriteLine("API URI " + builder.Configuration["PathApiBaseUri"]);

    if (string.IsNullOrEmpty(builder.Configuration["PathApiBaseUri"]))
    {
      throw new Exception("Path API base URL is null");
    }

    builder.Services.AddHttpClient("api",
            client => client.BaseAddress = new Uri(builder.Configuration["PathApiBaseUri"]))
        .AddHttpMessageHandler(sp =>
        {
          var handler = sp.GetService<AuthorizationMessageHandler>()
                    .ConfigureHandler(
                        authorizedUrls: [builder.Configuration["PathApiBaseUri"]],  //API URL
                        scopes: ["pathapi"]);
          return handler;
        });

    builder.Services.AddHttpClient("api-anonymous",
        client => client.BaseAddress = new Uri(builder.Configuration["PathApiBaseUri"]));

    builder.Services.AddLocalization();

    builder.Services
        .AddScoped(sp => sp.GetService<IHttpClientFactory>()
        .CreateClient("api"));

    builder.Services.AddMudServices(
        configuration =>
        {
          configuration.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
          configuration.SnackbarConfiguration.HideTransitionDuration = 500;
          configuration.SnackbarConfiguration.ShowTransitionDuration = 500;
          configuration.SnackbarConfiguration.ShowCloseIcon = true;
          configuration.SnackbarConfiguration.PositionClass = "mud-snackbar-location-bottom-right";
          configuration.SnackbarConfiguration.PreventDuplicates = true;
          configuration.SnackbarConfiguration.BackgroundBlurred = true;
          configuration.SnackbarConfiguration.VisibleStateDuration = 2000;
        }
    );

    builder.Services
        .AddOidcAuthentication(options =>
        {
          builder.Configuration.Bind("oidc", options.ProviderOptions);
          options.UserOptions.RoleClaim = "role";
        })
        .AddAccountClaimsPrincipalFactory<ArrayClaimsPrincipalFactory<RemoteUserAccount>>();

    // register services
    // common services
    builder.Services.AddScoped<HttpService>();
    builder.Services.AddScoped<SnackbarHelper>();
    builder.Services.AddScoped(state => new AppState(new ThemesHelper()));
    // user services
    builder.Services.AddTransient<Services.Common.PathService>();
    // administration services
    builder.Services.AddTransient<PathService>();
    builder.Services.AddTransient<ModuleService>();

    var host = builder.Build();

    await host.SetDefaultCulture();
    await host.SetCurrentTheme(host.Services.GetService<AppState>());
    await host.RunAsync();
  }
}
