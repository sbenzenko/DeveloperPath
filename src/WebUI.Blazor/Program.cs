using System;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using WebUI.Blazor.Security;
 
using MudBlazor.Services;
using WebUI.Blazor.Extensions;
using WebUI.Blazor.Services;

namespace WebUI.Blazor
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

            builder.Services.AddHttpClient("api", client =>client.BaseAddress = new Uri(builder.Configuration["PathApiBaseUri"]))
                .AddHttpMessageHandler(sp =>
                {
                    var handler = sp.GetService<AuthorizationMessageHandler>()
                        .ConfigureHandler(
                            authorizedUrls: new[] { builder.Configuration["PathApiBaseUri"] },  //API URL
                            scopes: new[] { "pathapi" });
                    return handler;
                });

            builder.Services.AddLocalization();

            builder.Services
                .AddScoped(sp => sp.GetService<IHttpClientFactory>()
                .CreateClient("api"));

            builder.Services.AddMudServices();
            builder.Services
                .AddOidcAuthentication(options =>
                {
                    builder.Configuration.Bind("oidc", options.ProviderOptions);
                    options.UserOptions.RoleClaim = "role";
                    
                })
                .AddAccountClaimsPrincipalFactory<ArrayClaimsPrincipalFactory<RemoteUserAccount>>();

            //register services
            builder.Services.AddTransient<PathService>();
            builder.Services.AddScoped<HttpService>();

            var host = builder.Build();
            await host.SetDefaultCulture();
            await host.RunAsync();
        }
    }
}
