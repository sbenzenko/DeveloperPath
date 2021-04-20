using AutoMapper;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Application.Common.Models;
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
  /// A module to create
  /// </summary>
  public record CreateModuleCommand : IRequest<ModuleDto>
  {
    // TODO: add Prerequisites, provide Order
    /// <summary>
    /// Id of path the module is in
    /// </summary>
    public int PathId { get; init; }
    /// <summary>
    /// Module title
    /// </summary>
    public string Title { get; init; }
    /// <summary>
    /// Path short summary
    /// </summary>
    public string Description { get; init; }
    /// <summary>
    /// Necessity level (Other (default) | Possibilities | Interesting | Good to know | Must know)
    /// </summary>
    public NecessityLevel Necessity { get; init; }
    /// <summary>
    /// Position of module in path (0-based)
    /// </summary>
    public int Order { get; init; }
    /// <summary>
    /// List of tags related to the module
    /// </summary>
    public IList<string> Tags { get; init; }
  }

  internal class CreateModuleCommandHandler : IRequestHandler<CreateModuleCommand, ModuleDto>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateModuleCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<ModuleDto> Handle(CreateModuleCommand request, CancellationToken cancellationToken)
    {
      var path = await _context.Paths
        .Where(c => c.Id == request.PathId)
        .FirstOrDefaultAsync(cancellationToken);

      if (path == null)
        throw new NotFoundException(nameof(Path), request.PathId);

      var entity = new Domain.Entities.Module
      {
        Title = request.Title,
        Description = request.Description,
        Necessity = request.Necessity,
        Tags = request.Tags,
        Paths = new List<Path> { path }
      };

      _context.Modules.Add(entity);

      await _context.SaveChangesAsync(cancellationToken);

      return _mapper.Map<ModuleDto>(entity);
    }
  }
}
