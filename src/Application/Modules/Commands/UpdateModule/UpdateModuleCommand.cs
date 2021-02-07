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
  public partial record UpdateModuleCommand : IRequest<ModuleDto>
  {
    // TODO: add Prerequisites, provide Order
    public int Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public NecessityLevel Necessity { get; init; }
    public int Order { get; init; }
    public IList<string> Tags { get; init; }
  }

  public class UpdateModuleCommandHandler : IRequestHandler<UpdateModuleCommand, ModuleDto>
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
