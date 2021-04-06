using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Application.Common.Models;
using DeveloperPath.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.Modules.Queries.GetModules
{
  public class GetModuleListQuery : IRequest<IEnumerable<ModuleDto>>
  {
    public int PathId { get; set; }
  }

  public class GetModulesQueryHandler : IRequestHandler<GetModuleListQuery, IEnumerable<ModuleDto>>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetModulesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<IEnumerable<ModuleDto>> Handle(GetModuleListQuery request, CancellationToken cancellationToken)
    {
      //TODO: check if requested module is in requested path (???)
      var path = await _context.Paths.FindAsync(new object[] { request.PathId }, cancellationToken);
      if (path == null)
        throw new NotFoundException(nameof(Path), request.PathId);

      // TODO: Order modules (from PathModules.Order)
      return await _context.Paths
        .Where(p => p.Id == request.PathId)
        .SelectMany(p => p.Modules)
        .Include(m => m.Paths)
        .Include(m => m.Prerequisites)
        .ProjectTo<ModuleDto>(_mapper.ConfigurationProvider)
        .ToListAsync(cancellationToken);
    }
  }
}
