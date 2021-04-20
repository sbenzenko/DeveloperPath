using AutoMapper;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Application.Common.Models;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Domain.Enums;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Application.Modules.Commands.UpdateModule
{
  /// <summary>
  /// Module to update
  /// </summary>
  public record UpdateModuleCommand : IRequest<ModuleDto>
  {
    // TODO: add Prerequisites, provide Order
    /// <summary>
    /// Module Id
    /// </summary>
    public int Id { get; init; }
    /// <summary>
    /// Module title
    /// </summary>
    public string Title { get; init; }
    /// <summary>
    /// Module short summary
    /// </summary>
    public string Description { get; init; }
    /// <summary>
    /// Necessity level (Other (default) | Possibilities | Interesting | Good to know | Must know)
    /// </summary>
    public NecessityLevel Necessity { get; init; }
    /// <summary>
    /// Order of module in path (0-based)
    /// </summary>
    public int Order { get; init; }
    /// <summary>
    /// List of tags related to the module
    /// </summary>
    public IList<string> Tags { get; init; }
  }

  internal class UpdateModuleCommandHandler : IRequestHandler<UpdateModuleCommand, ModuleDto>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateModuleCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<ModuleDto> Handle(UpdateModuleCommand request, CancellationToken cancellationToken)
    {
      var entity = await _context.Modules.FindAsync(new object[] { request.Id }, cancellationToken);
      if (entity == null)
        throw new NotFoundException(nameof(Module), request.Id);

      // TODO: is there a way to use init-only fields?
      entity.Title = request.Title;
      entity.Description = request.Description;
      entity.Necessity = request.Necessity;
      entity.Tags = request.Tags;

      await _context.SaveChangesAsync(cancellationToken);

      return _mapper.Map<ModuleDto>(entity);
    }
  }
}
