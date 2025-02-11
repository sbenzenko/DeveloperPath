using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Shared.ClientModels;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.CQRS.Modules.Queries.GetModules;

/// <summary>
/// Get module parameters
/// </summary>
public class GetModuleListQuery : IRequest<IEnumerable<Module>>
{
  /// <summary>
  /// URI key
  /// </summary>
  [Required]
  public string PathKey { get; init; }
}

internal class GetModulesQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetModuleListQuery, IEnumerable<Module>>
{
  private readonly IApplicationDbContext _context = context;
  private readonly IMapper _mapper = mapper;

  public async Task<IEnumerable<Module>> Handle(GetModuleListQuery request, CancellationToken cancellationToken)
  {
    var path = await _context.Paths.FirstOrDefaultAsync(x => x.Key == request.PathKey, cancellationToken: cancellationToken);
    if (path == null)
      throw new NotFoundException(nameof(Path), request.PathKey, NotFoundHelper.PATH_NOT_FOUND);

    return await _context.Paths
                 .AsNoTracking()
                 .Where(p => p.Key == request.PathKey)
                 .Include(p => p.PathModules.OrderBy(pm => pm.Order))
                  .ThenInclude(pm => pm.Module)
                 .ProjectTo<Module>(_mapper.ConfigurationProvider)
                 .ToListAsync(cancellationToken);
  }
}