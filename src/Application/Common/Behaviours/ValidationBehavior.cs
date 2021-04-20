using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using ValidationException = DeveloperPath.Application.Common.Exceptions.ValidationException;

namespace DeveloperPath.Application.Common.Behaviours
{
  /// <summary>
  /// Mediator pipilene behaviour for validation
  /// </summary>
  /// <typeparam name="TRequest"></typeparam>
  /// <typeparam name="TResponse"></typeparam>
  public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
      where TRequest : IRequest<TResponse>
  {
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    /// <summary>
    /// Ctor for injecting dependencies
    /// </summary>
    /// <param name="validators"></param>
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
      _validators = validators;
    }

    /// <summary>
    /// Pipeline handler. Perform any additional behavior and await the next delegate as necessary
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
      if (_validators.Any())
      {
        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

        if (failures.Count != 0)
          throw new ValidationException(failures);
      }
      return await next();
    }
  }
}