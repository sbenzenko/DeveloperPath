using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace DeveloperPath.WebApi.Extensions
{
  internal static class DependencyInjection
  {
    internal static IServiceCollection AddApiConfiguration(this IServiceCollection services)
    {
      services.AddApiControllers();
      services.ConfigureBehaviour();
      services.AddVersioning();
      services.AddSwaggerGenConfiguration();

      return services;
    }

    private static IServiceCollection ConfigureBehaviour(this IServiceCollection services)
    {
      // Customise default API behaviour
      services.Configure<ApiBehaviorOptions>(options =>
      {
        //options.SuppressModelStateInvalidFilter = true;

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

      return services;
    }

    private static IServiceCollection AddApiControllers(this IServiceCollection services)
    {
      services.AddControllers(options =>
      {
        options.ReturnHttpNotAcceptable = true;
        options.Filters.Add(
          new ProducesAttribute("application/json", "application/xml"));
      })
        .AddXmlDataContractSerializerFormatters()
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
      var apiVersionDescriptionProvider =
        services.BuildServiceProvider().GetService<IApiVersionDescriptionProvider>();

      services.AddSwaggerGen(setupAction =>
      {
        // Add swagger documents for all avaliable api versions
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
          setupAction.SwaggerDoc($"DeveloperPathAPISpecification{description.GroupName}",
                new Microsoft.OpenApi.Models.OpenApiInfo()
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

        setupAction.AddSecurityDefinition("bearerAuth",
          new OpenApiSecurityScheme()
          {
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Description = "Provide a JWT bearer token to access the API"
          });

        setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
          {
            new OpenApiSecurityScheme
            {
              Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "bearerAuth"
              }
            },
            new List<string>()
          }
        });

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
}
