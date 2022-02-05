using System;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Application.Common.Interfaces.Validation;

/// <summary>
/// Validation for unique URI key and Title
/// </summary>
public interface IUniqueNamingValidation
{
  /// <summary>
  /// Request to context to check for unique URI key
  /// </summary>
  /// <param name="id">Path Id</param>
  /// <param name="key"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  Task<bool> BeUniqueKey(Guid? id, string key, CancellationToken cancellationToken);

  /// <summary>
  /// Request to context to check for unique title
  /// </summary>
  /// <param name="id">Path Id</param>
  /// <param name="title"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  Task<bool> BeUniqueTitle(Guid? id, string title, CancellationToken cancellationToken);
}