using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DeveloperPath.Application;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Infrastructure;
using DeveloperPath.Infrastructure.Persistence;
using DeveloperPath.WebApi.Extensions;
using DeveloperPath.WebApi.Services;
using Microsoft.IdentityModel.Tokens;
using IdentityModel;
using Microsoft.ApplicationInsights.Extensibility;

[assembly: ApiConventionType(typeof(ApiCustomConventions))]
namespace DeveloperPath.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();
            services.AddInfrastructure(Configuration);

            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddHttpContextAccessor();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["Authority"] ?? throw new Exception("Value can't be null: Authority");
                    options.Audience = Configuration["Audience"] ?? throw new Exception("Value can't be null: Audience");
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        NameClaimType = "name"
                    };
                });

            services.AddApiConfiguration();

            services.AddHealthChecks()
                    .AddDbContextCheck<ApplicationDbContext>();

            services.AddScoped<TelemetryConfiguration>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //todo: fix CORS
            app.UseCors(config =>
            {
                config.AllowAnyOrigin();
                config.AllowAnyMethod();
                config.AllowAnyHeader();
                config.WithExposedHeaders("*"); //needs for blazor to read headers
            });



            app.UseHealthChecks("/health");

            app.UseHttpsRedirection();

            app.UseSwagger();
            var secret = Configuration["PathApiSwaggerSecret"] ?? throw new Exception("Value can't be null: PathApiSwaggerSecret");
            app.UseSwaggerUI(settings =>
            {
                settings.OAuthClientId("swagger");
                settings.OAuthAppName("Developer Path API - Swagger");
                settings.OAuthUsePkce();
                settings.OAuthClientSecret(secret);
                 
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    settings.RoutePrefix = "";
                    settings.SwaggerEndpoint(
                $"/swagger/DeveloperPathAPISpecification{description.GroupName}/swagger.json",
                $"DeveloperPath API {description.GroupName.ToUpperInvariant()}");
                }
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
