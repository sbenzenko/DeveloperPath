using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Application.Themes.Commands.DeleteTheme
{
  public record DeleteThemeCommand : IRequest
  {
    public int Id { get; init; }
    public int ModuleId { get; init; }
  }

  public class DeleteThemeCommandHandler : IRequestHandler<DeleteThemeCommand>
  {
    private readonly IApplicationDbContext _context;

    public DeleteThemeCommandHandler(IApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<Unit> Handle(DeleteThemeCommand request, CancellationToken cancellationToken)
    {
      var entity = await _context.Themes
        .Where(t => t.Id == request.Id && t.ModuleId == request.ModuleId)
        .FirstOrDefaultAsync(cancellationToken);

      if (entity == null)
      {
        throw new NotFoundException(nameof(Theme), request.Id);
      }

      _context.Themes.Remove(entity);

      await _context.SaveChangesAsync(cancellationToken);

      return Unit.Value;
    }
  }
}
