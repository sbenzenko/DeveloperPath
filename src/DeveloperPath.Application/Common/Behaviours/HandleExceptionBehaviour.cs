using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Application.Common.Behaviours
{
  /// <summary>
  /// Mediator pipilene behaviour for handling exceptions
  /// </summary>
  /// <typeparam name="TRequest"></typeparam>
  /// <typeparam name="TResponse"></typeparam>
  public class HandleExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : IRequest<TResponse>
  {
    private readonly ILogger<TRequest> _logger;

    /// <summary>
    /// Ctor for injecting dependencies
    /// </summary>
    /// <param name="logger"></param>
    public HandleExceptionBehaviour(ILogger<TRequest> logger)
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
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
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
}
