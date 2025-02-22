﻿using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Entities;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.CQRS.Modules.Commands.DeleteModule;

/// <summary>
/// Delete module parameters
/// </summary>
public record DeleteModule : IRequest<Module>
{
  /// <summary>
  /// Module Id
  /// </summary>
  [Required]
  public int Id { get; init; }
  /// <summary>
  /// Path id
  /// </summary>
  [Required]
  public int PathId { get; init; }
}

internal class DeleteModuleCommandHandler : IRequestHandler<DeleteModule, Module>
{
  private readonly IApplicationDbContext _context;

  public DeleteModuleCommandHandler(IApplicationDbContext context)
  {
    _context = context;
  }

  public async Task<Module> Handle(DeleteModule request, CancellationToken cancellationToken)
  {
    var entity = await _context.Modules
      .Include(m => m.Paths)
      .Where(m => m.Id == request.Id)
      .FirstOrDefaultAsync(cancellationToken);

    if (entity == null)
      throw new NotFoundException(nameof(Module), request.Id, NotFoundHelper.MODULE_NOT_FOUND);

    if (entity.Paths.Any(p => p.Id == request.PathId))
    {
      // remove Module from this path
      entity.Paths.Remove(entity.Paths.Single(p => p.Id == request.PathId));

      // If module is not tied to any path, delete the module
      if (entity.Paths.Count == 0)
        _context.Modules.Remove(entity);

      await _context.SaveChangesAsync(cancellationToken);
    }
    else
      throw new NotFoundException(nameof(Module), request.Id, NotFoundHelper.MODULE_NOT_FOUND);

    return entity;
  }
}
