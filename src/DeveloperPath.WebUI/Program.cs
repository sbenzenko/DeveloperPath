using System;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using MudBlazor;
using MudBlazor.Services;
using DeveloperPath.WebUI.Services;
using DeveloperPath.WebUI.UIHelpers;
using DeveloperPath.WebUI.Extensions;
using DeveloperPath.WebUI.Security;

namespace DeveloperPath.WebUI
{
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
                            authorizedUrls: new[] { builder.Configuration["PathApiBaseUri"] },  //API URL
                            scopes: new[] { "pathapi" });
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

            //register services
            builder.Services.AddTransient<PathService>();
            builder.Services.AddTransient<ModuleService>();
            builder.Services.AddScoped<HttpService>();
            builder.Services.AddScoped<SnackbarHelper>();
            builder.Services.AddScoped(state => new AppState(new ThemesHelper()));

            var host = builder.Build();

            await host.SetDefaultCulture();
            await host.SetCurrentTheme(host.Services.GetService<AppState>());
            await host.RunAsync();
        }
    }
}
