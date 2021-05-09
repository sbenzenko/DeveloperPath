using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Application.CQRS.Sources.Commands.DeleteSource
{
  /// <summary>
  /// Delete source parameters
  /// </summary>
  public record DeleteSource : IRequest
  {
    /// <summary>
    /// Source Id
    /// </summary>
    [Required]
    public int Id { get; init; }
    /// <summary>
    /// Path Id
    /// </summary>
    [Required]
    public int PathId { get; init; }
    /// <summary>
    /// Module Id
    /// </summary>
    [Required]
    public int ModuleId { get; init; }
    /// <summary>
    /// Theme Id
    /// </summary>
    [Required]
    public int ThemeId { get; init; }
  }

  internal class DeleteSourceCommandHandler : IRequestHandler<DeleteSource>
  {
    private readonly IApplicationDbContext _context;

    public DeleteSourceCommandHandler(IApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<Unit> Handle(DeleteSource request, CancellationToken cancellationToken)
    {
      //TODO: check if requested module is in requested path (???)
      var path = await _context.Paths.FindAsync(new object[] { request.PathId }, cancellationToken);
      if (path == null)
        throw new NotFoundException(nameof(Path), request.PathId, NotFoundHelper.PATH_NOT_FOUND);

      var theme = await _context.Themes
        .Where(t => t.Id == request.ThemeId && t.ModuleId == request.ModuleId)
        .FirstOrDefaultAsync(cancellationToken);
      if (theme == null)
        throw new NotFoundException(nameof(Theme), request.ThemeId, NotFoundHelper.THEME_NOT_FOUND);

      var entity = await _context.Sources
        .Where(t => t.Id == request.Id && t.ThemeId == request.ThemeId)
        .FirstOrDefaultAsync(cancellationToken);
      if (entity == null)
        throw new NotFoundException(nameof(Source), request.Id, NotFoundHelper.SOURCE_NOT_FOUND);

      _context.Sources.Remove(entity);

      await _context.SaveChangesAsync(cancellationToken);

      return Unit.Value;
    }
  }
}
