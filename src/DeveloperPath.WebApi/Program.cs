using System;
using System.Threading.Tasks;
using Azure.Identity;
using DeveloperPath.BuildInfo;
using DeveloperPath.Infrastructure.Persistence;
using Microsoft.ApplicationInsights.Extensibility;
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
            var buildInfo = AppVersionInfo.GetBuildInfo();

            var baseLoggerConfig = new LoggerConfiguration()
              .MinimumLevel.Debug()
               .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
               .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
               .MinimumLevel.Override("System", LogEventLevel.Warning)
               .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)

               .Enrich.FromLogContext()
               .Enrich.WithProperty("ApplicationName", APP_NAME)
               .Enrich.WithProperty(nameof(buildInfo.BuildId), buildInfo.BuildId)
               .Enrich.WithProperty(nameof(buildInfo.BuildNumber), buildInfo.BuildNumber)
               .Enrich.WithProperty(nameof(buildInfo.BranchName), buildInfo.BranchName)
               .Enrich.WithProperty(nameof(buildInfo.CommitHash), buildInfo.CommitHash)
               .WriteTo.File(
                   @"D:\home\LogFiles\Application\webapi.txt",
                   fileSizeLimitBytes: 1_000_000,
                   rollOnFileSizeLimit: true,
                   shared: true,
                   flushToDiskInterval: TimeSpan.FromSeconds(1))
               .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code);



            try
            {
                var host = CreateHostBuilder(args).Build();
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var env = services.GetService<IWebHostEnvironment>();
                    var configuration = services.GetService<IConfiguration>();
                    if (env.IsProduction() && !string.IsNullOrWhiteSpace(configuration.GetValue<string>("APPINSIGHTS_INSTRUMENTATIONKEY")))
                    {
                        var telemetry = services.GetRequiredService<TelemetryConfiguration>();
                        baseLoggerConfig = baseLoggerConfig.WriteTo.ApplicationInsights(telemetry, TelemetryConverter.Traces);
                    }

                    Log.Logger = baseLoggerConfig.CreateLogger();

                    var logger = services.GetRequiredService<ILogger<Program>>();

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
