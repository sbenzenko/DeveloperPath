using System;
using System.Threading.Tasks;
using Azure.Identity;
using DeveloperPath.BuildInfo;
using DeveloperPath.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace DeveloperPath.WebApi
{
    public class Program
    {
        const string APP_NAME = "DeveloperPath.WebApi";
        public async static Task Main(string[] args)
        {
            AppVersionInfo.InitialiseBuildInfoGivenPath(AppDomain.CurrentDomain.BaseDirectory);
            Log.Logger = new LoggerConfiguration()
              .MinimumLevel.Debug()
               .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
               .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
               .MinimumLevel.Override("System", LogEventLevel.Warning)
               .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
               .Enrich.FromLogContext()
               // uncomment to write to Azure diagnostics stream
               .WriteTo.File(
                   @"D:\home\LogFiles\Application\webapi.txt",
                   fileSizeLimitBytes: 1_000_000,
                   rollOnFileSizeLimit: true,
                   shared: true,
                   flushToDiskInterval: TimeSpan.FromSeconds(1))
               .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
               .CreateLogger();


            try
            {
                var host = CreateHostBuilder(args).Build();
                using (var scope = host.Services.CreateScope())
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    var services = scope.ServiceProvider;
                    logger.LogInformation($"BaseDirectory: {AppDomain.CurrentDomain.BaseDirectory}");
                    try
                    {
                        var context = services.GetRequiredService<ApplicationDbContext>();

                        if (context.Database.IsSqlServer())
                        {
                            logger.LogInformation("DataBase migration..");
                            await context.Database.MigrateAsync();
                            logger.LogInformation("DataBase migrated");
                        }

                        logger.LogInformation("DataBase seeding..");
                        await ApplicationDbContextSeed.SeedSampleDataAsync(context);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred while migrating or seeding the database.");

                        throw;
                    }
                }
                Log.Information("Starting up..");
                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }


        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
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
