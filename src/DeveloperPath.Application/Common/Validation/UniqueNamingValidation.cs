using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Application.Common.Interfaces.Validation;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.Common.Validation;

/// <summary>
/// Base validation logic for path related to data base checking
/// </summary>
public class UniqueNamingValidation : IUniqueNamingValidation
{
  private readonly IApplicationDbContext _context;

  /// <summary>
  /// 
  /// </summary>
  /// <param name="context"></param>
  public UniqueNamingValidation(IApplicationDbContext context)
  {
    _context = context;
  }

  /// <summary>
  /// Request to context to check for unique URI key
  /// </summary>
  /// <param name="id">Path Id</param>
  /// <param name="key"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  public async Task<bool> BeUniqueKey(Guid? id, string key, CancellationToken cancellationToken)
  {
    var query = _context.Paths.AsQueryable();
    if (id != default)
      query = query.Where(p => p.Id != id);
    return await query.AllAsync(l => l.Key != key, cancellationToken);
  }

  /// <summary>
  /// Request to context to check for unique title
  /// </summary>
  /// <param name="id">Path Id</param>
  /// <param name="title"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  public async Task<bool> BeUniqueTitle(Guid? id, string title, CancellationToken cancellationToken)
  {
    var query = _context.Paths.AsQueryable();
    if (id != default)
      query = query.Where(p => p.Id != id);
    return await query.AllAsync(l => l.Title != title, cancellationToken);
  }
}
