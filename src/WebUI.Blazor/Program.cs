using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebUI.Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            
            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("oidc", options.ProviderOptions);
                
                Console.WriteLine($"{nameof(options.ProviderOptions.Authority)}:{options.ProviderOptions.Authority}");
                Console.WriteLine($"{nameof(options.ProviderOptions.ClientId)}:{options.ProviderOptions.ClientId}");
                
            });
          
            await builder.Build().RunAsync();
        }
    }
}
