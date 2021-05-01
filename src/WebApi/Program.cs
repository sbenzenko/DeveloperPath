using System;
using System.Threading.Tasks;
using Azure.Identity;
using DeveloperPath.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DeveloperPath.WebApi
{
  public class Program
  {
    public async static Task Main(string[] args)
    {
      var host = CreateHostBuilder(args).Build();

      using (var scope = host.Services.CreateScope())
      {
        var services = scope.ServiceProvider;

        try
        {
          var context = services.GetRequiredService<ApplicationDbContext>();

          if (context.Database.IsSqlServer())
          {
            context.Database.Migrate();
          }

         
          await ApplicationDbContextSeed.SeedSampleDataAsync(context);
        }
        catch (Exception ex)
        {
          var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

          logger.LogError(ex, "An error occurred while migrating or seeding the database.");

          throw;
        }
      }

      await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
                webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                {
                    if (hostingContext.HostingEnvironment.IsProduction())
                    {
                        var settings = config.Build();

                        if (hostingContext.HostingEnvironment.IsDevelopment())
                        {

                        }

                        if (hostingContext.HostingEnvironment.IsProduction())
                        {
                            config.AddAzureAppConfiguration(options =>
                            {
                                options.Connect(settings["ConnectionStrings:AppConfig"])
                                    .ConfigureKeyVault(kv =>
                                    {
                                        kv.SetCredential(new DefaultAzureCredential());
                                    });
                            });
                        }
                    }
                }).UseStartup<Startup>());
    }
}
