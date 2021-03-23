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
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Application.Modules.Commands.CreateModule
{
  /// <summary>
  /// Represents developer module entity
  /// </summary>
  public record CreateModuleCommand : IRequest<ModuleDto>
  {
    // TODO: add Prerequisites, provide Order
    public int PathId { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public NecessityLevel Necessity { get; init; }
    public int Order { get; init; }
    public IList<string> Tags { get; init; }
  }

  public class CreateModuleCommandHandler : IRequestHandler<CreateModuleCommand, ModuleDto>
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

      var entity = new DeveloperPath.Domain.Entities.Module
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
