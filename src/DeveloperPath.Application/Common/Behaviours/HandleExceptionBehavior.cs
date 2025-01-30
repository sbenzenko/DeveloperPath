using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

namespace DeveloperPath.Application.Common.Behaviors;

/// <summary>
/// Mediator pipeline behavior for handling exceptions
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class HandleExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
  private readonly ILogger<TRequest> _logger;

  /// <summary>
  /// Ctor for injecting dependencies
  /// </summary>
  /// <param name="logger"></param>
  public HandleExceptionBehavior(ILogger<TRequest> logger)
  {
    _logger = logger;
  }

  /// <summary>
  /// Pipeline handler. Perform any additional behavior and await the next delegate as necessary
  /// </summary>
  /// <param name="request"></param>
  /// <param name="cancellationToken"></param>
  /// <param name="next"></param>
  /// <returns></returns>
  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    try
    {
      return await next();
    }
    catch (Exception ex)
    {
      var requestName = typeof(TRequest).Name;
      if (ex is OperationCanceledException)
        _logger.LogWarning("DeveloperPath Request: {@Request} was canceled", requestName);
      else
        _logger.LogError(ex, "DeveloperPath Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);

      throw;
    }
  }
}