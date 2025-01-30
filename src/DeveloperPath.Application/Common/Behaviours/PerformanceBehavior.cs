using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using DeveloperPath.Application.Common.Interfaces;

using MediatR;

using Microsoft.Extensions.Logging;

namespace DeveloperPath.Application.Common.Behaviors;

/// <summary>
/// Mediator pipeline behavior for measuring performance
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
  private readonly Stopwatch _timer;
  private readonly ILogger<TRequest> _logger;
  private readonly ICurrentUserService _currentUserService;
  //private readonly IIdentityService _identityService;

  /// <summary>
  /// Ctor for injecting dependencies
  /// </summary>
  /// <param name="logger"></param>
  /// <param name="currentUserService"></param>
  ///// <param name="identityService"></param>
  public PerformanceBehavior(
      ILogger<TRequest> logger,
      ICurrentUserService currentUserService)
  {
    _timer = new Stopwatch();

    _logger = logger;
    _currentUserService = currentUserService;
    // _identityService = identityService;
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
    _timer.Start();

    var response = await next();

    _timer.Stop();

    var elapsedMilliseconds = _timer.ElapsedMilliseconds;

    if (elapsedMilliseconds > 500)
    {
      var requestName = typeof(TRequest).Name;
      var userId = _currentUserService.UserId ?? string.Empty;
      var userName = string.Empty;

      if (!string.IsNullOrEmpty(userId))
      {
        // userName = await _identityService.GetUserNameAsync(userId);
      }

      _logger.LogWarning("DeveloperPath Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
          requestName, elapsedMilliseconds, userId, userName, request);
    }

    return response;
  }
}
