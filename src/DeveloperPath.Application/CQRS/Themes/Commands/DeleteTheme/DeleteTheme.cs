﻿using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Entities;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.CQRS.Themes.Commands.DeleteTheme;

/// <summary>
/// Delete theme parameters
/// </summary>
public record DeleteTheme : IRequest<Theme>
{
  /// <summary>
  /// Theme id
  /// </summary>
  [Required]
  public int Id { get; init; }
  /// <summary>
  /// Path id
  /// </summary>
  [Required]
  public int PathId { get; init; }
  /// <summary>
  /// Module id
  /// </summary>
  [Required]
  public int ModuleId { get; init; }
}

internal class DeleteThemeCommandHandler : IRequestHandler<DeleteTheme, Theme>
{
  private readonly IApplicationDbContext _context;

  public DeleteThemeCommandHandler(IApplicationDbContext context)
  {
    _context = context;
  }

  public async Task<Theme> Handle(DeleteTheme request, CancellationToken cancellationToken)
  {
    //TODO: check if requested module is in requested path (???)
    var path = await _context.Paths.FindAsync(new object[] { request.PathId }, cancellationToken);
    if (path == null)
      throw new NotFoundException(nameof(Path), request.PathId, NotFoundHelper.PATH_NOT_FOUND);

    var entity = await _context.Themes
      .Where(t => t.Id == request.Id && t.ModuleId == request.ModuleId)
      .FirstOrDefaultAsync(cancellationToken);
    if (entity == null)
      throw new NotFoundException(nameof(Theme), request.Id, NotFoundHelper.THEME_NOT_FOUND);

    _context.Themes.Remove(entity);

    await _context.SaveChangesAsync(cancellationToken);

    return entity;
  }
}