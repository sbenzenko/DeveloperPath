using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Application.Themes.Commands.UpdateTheme
{

  /// <summary>
  /// Represents developer theme entity
  /// </summary>
  public partial record UpdateThemeCommand : IRequest
  {
    public int Id { get; init; }
    public int ModuleId { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public int SectionId { get; init; }
    public ComplexityLevel Complexity { get; init; }
    public NecessityLevel Necessity { get; init; }
    public int Order { get; init; }
  }

  public class UpdateThemeCommandHandler : IRequestHandler<UpdateThemeCommand>
  {
    private readonly IApplicationDbContext _context;

    public UpdateThemeCommandHandler(IApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<Unit> Handle(UpdateThemeCommand request, CancellationToken cancellationToken)
    {
      var entity = await _context.Themes.FindAsync(request.Id, cancellationToken);
      if (entity == null)
        throw new NotFoundException(nameof(Theme), request.Id);

      var module = await _context.Modules
        .Where(m => m.Id == request.ModuleId)
        .FirstOrDefaultAsync(cancellationToken);
      if (module == null)
        throw new NotFoundException(nameof(Module), request.ModuleId);

      Section section = null;
      if (request.SectionId > 0)
      {
        section = await _context.Sections
          .Where(s => s.Id == request.SectionId)
          .FirstOrDefaultAsync(cancellationToken);

        if (section == null)
          throw new NotFoundException(nameof(Section), request.SectionId);
      }

      // TODO: is there a way to use init-only fields?
      entity.Title = request.Title;
      entity.Description = request.Description;
      entity.ModuleId = request.ModuleId;
      entity.Section = section;
      entity.Complexity = request.Complexity;
      entity.Necessity = request.Necessity;
      entity.Order = request.Order;

      await _context.SaveChangesAsync(cancellationToken);

      return Unit.Value;
    }
  }
}
