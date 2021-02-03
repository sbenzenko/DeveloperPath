using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Application.Sources.Commands.DeleteSource
{
  public record DeleteSourceCommand : IRequest
  {
    public int Id { get; init; }
    public int ModuleId { get; init; }
    public int ThemeId { get; init; }
  }

  public class DeleteSourceCommandHandler : IRequestHandler<DeleteSourceCommand>
  {
    private readonly IApplicationDbContext _context;

    public DeleteSourceCommandHandler(IApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<Unit> Handle(DeleteSourceCommand request, CancellationToken cancellationToken)
    {
      var module = await _context.Modules.FindAsync(new object[] { request.ModuleId }, cancellationToken);
      if (module == null)
        throw new NotFoundException(nameof(Module), request.ModuleId);

      var entity = await _context.Sources
        .Where(t => t.Id == request.Id && t.ThemeId == request.ThemeId)
        .FirstOrDefaultAsync(cancellationToken);
      if (entity == null)
        throw new NotFoundException(nameof(Source), request.Id);

      _context.Sources.Remove(entity);

      await _context.SaveChangesAsync(cancellationToken);

      return Unit.Value;
    }
  }
}
