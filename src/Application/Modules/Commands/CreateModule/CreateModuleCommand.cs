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

namespace DeveloperPath.Application.Modules.Commands.CreateModule
{

  /// <summary>
  /// Represents developer module entity
  /// </summary>
  public record CreateModuleCommand : IRequest<int>
  {
    public int PathId { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public IList<string> Tags { get; init; }
    public NecessityLevel Necessity { get; init; }
  }

  public class CreateModuleCommandHandler : IRequestHandler<CreateModuleCommand, int>
  {
    private readonly IApplicationDbContext _context;

    public CreateModuleCommandHandler(IApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<int> Handle(CreateModuleCommand request, CancellationToken cancellationToken)
    {
      var path = await _context.Paths
        .Where(c => c.Id == request.PathId)
        .FirstOrDefaultAsync(cancellationToken);

      if (path == null)
      {
        throw new NotFoundException(nameof(Path), request.PathId);
      }

      var entity = new Module
      {
        Title = request.Title,
        Description = request.Description,
        Necessity = request.Necessity,
        Tags = request.Tags,
        Paths = new List<Path> { path }
      };

      _context.Modules.Add(entity);

      await _context.SaveChangesAsync(cancellationToken);

      return entity.Id;
    }
  }
}
