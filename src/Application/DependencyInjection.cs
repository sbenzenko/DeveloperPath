using DeveloperPath.Application.Common.Behaviours;
using DeveloperPath.Application.Common.Mappings.Interfaces;
using DeveloperPath.Application.Common.Mappings.Profiles;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using DeveloperPath.Application.CQRS.Paths.Commands;

namespace DeveloperPath.Application
{
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
      services.AddMediatR(Assembly.GetExecutingAssembly());

      services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
      services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
      services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
      services.AddScoped<IBasePathValidation, BasePathValidation>();
      return services;
    }
  }
}
