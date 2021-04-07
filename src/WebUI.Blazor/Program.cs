using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using WebUI.Blazor.Security;
using MatBlazor;
using WebUI.Blazor.Extensions;

namespace WebUI.Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            // We register a named HttpClient here for the API
            builder.Services.AddHttpClient("pathapi")
                .AddHttpMessageHandler(sp =>
                {
                    var handler = sp.GetService<AuthorizationMessageHandler>()
                        .ConfigureHandler(
                            authorizedUrls: new[] { "https://localhost:7001" },  //API URL
                            scopes: new[] { "pathapi" });
                    return handler;
                });

            builder.Services.AddLocalization();

            builder.Services.AddScoped(sp => sp.GetService<IHttpClientFactory>().CreateClient("pathapi"));
            builder.Services.AddMatBlazor();
            builder.Services
                .AddOidcAuthentication(options =>
                {
                    options.ProviderOptions.Authority = "https://devpathprovider.com";
                    options.ProviderOptions.ClientId = "WebUI.Blazor";
                    options.ProviderOptions.DefaultScopes.Add("openid");
                    options.ProviderOptions.DefaultScopes.Add("profile");
                    options.ProviderOptions.DefaultScopes.Add("pathapi");
                    options.ProviderOptions.PostLogoutRedirectUri = "/";
                    options.ProviderOptions.ResponseType = "code";
                    options.UserOptions.RoleClaim = "role";
                })
                .AddAccountClaimsPrincipalFactory<ArrayClaimsPrincipalFactory<RemoteUserAccount>>();

            var host = builder.Build();
            await host.SetDefaultCulture();
            await host.RunAsync();
        }
    }
}
