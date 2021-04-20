using System;
using System.Reflection;
using DeveloperPath.Application;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Infrastructure;
using DeveloperPath.Infrastructure.Persistence;
using DeveloperPath.WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

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
            services.AddControllers(options => options.ReturnHttpNotAcceptable = true)
                .AddXmlDataContractSerializerFormatters();

            services.AddHealthChecks()
                    .AddDbContextCheck<ApplicationDbContext>();

            // Customise default API behaviour
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;

                // in case of Model validation error return HTTP 422 instead of 401
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var actionExecutingContext =
                      actionContext as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;

                    // if there are modelstate errors & all keys were correctly
                    // found/parsed we're dealing with validation errors
                    if (actionContext.ModelState.ErrorCount > 0
                        && actionExecutingContext?.ActionArguments.Count == actionContext.ActionDescriptor.Parameters.Count)
                    {
                        return new UnprocessableEntityObjectResult(actionContext.ModelState);
                    }

                    // if one of the keys wasn't correctly found / couldn't be parsed
                    // we're dealing with null/unparsable input
                    return new BadRequestObjectResult(actionContext.ModelState);
                };
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://localhost:6001";
                    options.Audience = "pathapi";
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        NameClaimType = "name"
                    };
                });

            services.AddSwaggerGen(setupAction =>
            {
              setupAction.SwaggerDoc("DeveloperPathAPISpecification",
                new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                  Version = "v1",
                  Title = "Developer Path API",
                  Description = "Developer Path project Open API specification",
                  Contact = new Microsoft.OpenApi.Models.OpenApiContact
                  {
                    Name = "Sergey Benzenko",
                    Email = "sbenzenko@gmail.com",
                    Url = new System.Uri("https://t.me/NetDeveloperDiary")
                  }
                });

              setupAction.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, "DeveloperPath.Api.xml"));
              setupAction.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, "DeveloperPath.Models.xml"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseCors(config =>
            {
                config.AllowAnyOrigin();
                config.AllowAnyMethod();
                config.AllowAnyHeader();
            });

            app.UseHealthChecks("/health");

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(settings =>
            {
                settings.RoutePrefix = "";
                settings.SwaggerEndpoint("/swagger/DeveloperPathAPISpecification/swagger.json", "DeveloperPath API");
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
