using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Domain.Enums;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Application.Modules.Commands.UpdateModule
{
  /// <summary>
  /// Represents developer module entity
  /// </summary>
  public partial record UpdateModuleCommand : IRequest
  {
    public int Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public NecessityLevel Necessity { get; init; }
    public IList<string> Tags { get; init; }
  }

  public class UpdateModuleCommandHandler : IRequestHandler<UpdateModuleCommand>
  {
    private readonly IApplicationDbContext _context;

    public UpdateModuleCommandHandler(IApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<Unit> Handle(UpdateModuleCommand request, CancellationToken cancellationToken)
    {
      var entity = await _context.Modules.FindAsync(request.Id, cancellationToken);

      if (entity == null)
      {
        throw new NotFoundException(nameof(Module), request.Id);
      }

      // TODO: is there a way to use init-only fields?
      entity.Title = request.Title;
      entity.Description = request.Description;
      entity.Necessity = request.Necessity;
      entity.Tags = request.Tags;

      await _context.SaveChangesAsync(cancellationToken);

      return Unit.Value;
    }
  }
}
