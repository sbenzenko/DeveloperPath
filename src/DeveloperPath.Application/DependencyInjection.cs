using System.Reflection;

using DeveloperPath.Application.Common.Behaviors;
using DeveloperPath.Application.Common.Mappings.Interfaces;
using DeveloperPath.Application.Common.Mappings.Profiles;
using DeveloperPath.Application.CQRS.Paths.Commands;

using FluentValidation;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace DeveloperPath.Application;

/// <summary>
/// </summary>
public static class DependencyInjection
{
  /// <summary>
  /// Extension method for startup class to add application services
  /// </summary>
  /// <param name="services"></param>
  /// <returns></returns>
  public static IServiceCollection AddApplication(this IServiceCollection services)
  {
    services.AddAutoMapper(typeof(PathProfile).Assembly);
    services.AddAutoMapper(typeof(MappingProfile).Assembly);

    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    services.AddMediatR(cfg =>
     cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(HandleExceptionBehavior<,>));
    services.AddScoped<IBasePathValidation, BasePathValidation>();
    return services;
  }
}
