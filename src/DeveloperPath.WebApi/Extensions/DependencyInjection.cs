using System;
using System.Collections.Generic;
using System.Linq;

using DeveloperPath.Shared.ProblemDetails;
using DeveloperPath.WebApi.Filters;
using DeveloperPath.WebApi.Helpers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;


namespace DeveloperPath.WebApi.Extensions;

internal static class DependencyInjection
{
  internal static IServiceCollection AddApiConfiguration(this IServiceCollection services)
  {
    services.AddApiControllers();
    services.ConfigureBehavior();
    services.AddVersioning();
    services.AddSwaggerGenConfiguration();

    services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
    services.AddScoped<PagedListResultFilterAttribute>();
    services.AddScoped<PagedListHeadersHelper>();

    return services;
  }

  private static IServiceCollection ConfigureBehavior(this IServiceCollection services)
  {
    // Customise default API behavior
    services.Configure<ApiBehaviorOptions>(options =>
    {
      //options.SuppressModelStateInvalidFilter = true;
      // in case of Model validation error return HTTP 422 instead of 401
      options.InvalidModelStateResponseFactory = actionContext =>
            {
              var actionExecutingContext =
                      actionContext as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;

              // if there are model state errors & all keys were correctly
              // found/parsed we're dealing with validation errors
              if (actionContext.ModelState.ErrorCount > 0
                        && actionExecutingContext?.ActionArguments.Count == actionContext.ActionDescriptor.Parameters.Count)
              {

                return new UnprocessableEntityObjectResult(
                          new UnprocessableEntityProblemDetails
                          {
                            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                            Detail = "See the errors property for details",
                            Instance = actionExecutingContext.HttpContext.Request.Path,
                            Errors = actionContext.ModelState.ToDictionary(x => x.Key, pair => pair.Value.Errors.Select(x => x.ErrorMessage).ToArray())
                          });
              }
              // if one of the keys wasn't correctly found / couldn't be parsed
              // we're dealing with null/unparsable input
              return new BadRequestObjectResult(actionContext.ModelState);
            };
    });

    return services;
  }

  private static IServiceCollection AddApiControllers(this IServiceCollection services)
  {
    services.AddControllers(options =>
    {
      options.Filters.Add<ApiExceptionFilterAttribute>();
      options.ReturnHttpNotAcceptable = true;
      options.Filters.Add(
        new ProducesAttribute("application/json", "application/xml"));
    }).AddNewtonsoftJson()
      //.AddXmlDataContractSerializerFormatters()
      .AddJsonOptions(options =>
      {
        // serialize enums as strings
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
      });

    return services;
  }

  private static IServiceCollection AddVersioning(this IServiceCollection services)
  {
    services.AddVersionedApiExplorer(setupAction =>
    {
      setupAction.GroupNameFormat = "'v'VV";
    });

    services.AddApiVersioning(setupAction =>
    {
      setupAction.AssumeDefaultVersionWhenUnspecified = true;
      setupAction.DefaultApiVersion = new ApiVersion(1, 0);
      setupAction.ReportApiVersions = true;
      setupAction.ApiVersionReader = new HeaderApiVersionReader("api-version");
    });

    return services;
  }

  private static IServiceCollection AddSwaggerGenConfiguration(this IServiceCollection services)
  {
    var serviceProvider = services.BuildServiceProvider();
    var apiVersionDescriptionProvider = serviceProvider.GetService<IApiVersionDescriptionProvider>();
    var configuration = serviceProvider.GetService<IConfiguration>();


    services.AddSwaggerGen(setupAction =>
    {
      // Add swagger documents for all available api versions
      foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
      {
        setupAction.SwaggerDoc($"DeveloperPathAPISpecification{description.GroupName}",
              new OpenApiInfo()
              {
                Version = description.ApiVersion.ToString(),
                Title = "Developer Path API",
                Description = "Developer Path project Open API specification",
                Contact = new Microsoft.OpenApi.Models.OpenApiContact
                {
                  Name = "Sergey Benzenko",
                  Email = "sbenzenko@gmail.com",
                  Url = new Uri("https://t.me/NetDeveloperDiary")
                }
              });
      }
      var authority = configuration["Authority"] ?? throw new Exception("Value can't be null: Authority");
      setupAction.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
      {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
          AuthorizationCode = new OpenApiOAuthFlow
          {
            AuthorizationUrl = new Uri($"{authority}/connect/authorize"),
            TokenUrl = new Uri($"{authority}/connect/token"),
            Scopes = new Dictionary<string, string>
              {
                {"pathapi", "API - full access"}
              }
          }
        }
      });

      setupAction.OperationFilter<AuthorizeCheckOperationFilter>();

      // select swagger document based on selected API version
      setupAction.DocInclusionPredicate((documentName, apiDescription) =>
      {
        var actionApiVersionModel = apiDescription.ActionDescriptor
          .GetApiVersionModel(ApiVersionMapping.Explicit | ApiVersionMapping.Implicit);

        if (actionApiVersionModel is null)
          return true;

        if (actionApiVersionModel.DeclaredApiVersions.Any())
        {
          return actionApiVersionModel.DeclaredApiVersions.Any(v =>
            $"DeveloperPathAPISpecificationv{v}" == documentName);
        }
        return actionApiVersionModel.ImplementedApiVersions.Any(v =>
          $"DeveloperPathAPISpecificationv{v}" == documentName);
      });

      setupAction.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, "DeveloperPath.Api.xml"));
      setupAction.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, "DeveloperPath.Models.xml"));
    });

    return services;
  }
}