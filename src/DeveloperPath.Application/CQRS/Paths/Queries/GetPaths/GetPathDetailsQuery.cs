using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Shared.ClientModels;

using MediatR;

using Microsoft.EntityFrameworkCore;


namespace DeveloperPath.Application.CQRS.Paths.Queries.GetPaths;

/// <summary>
/// Get path details parameters
/// </summary>
public class GetPathDetailsQuery : IRequest<PathDetails>
{
  /// <summary>
  /// Path id
  /// </summary>
  [Required]
  public int Id { get; init; }
}

internal class GetPathDetailsQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetPathDetailsQuery, PathDetails>
{
  private readonly IApplicationDbContext _context = context;
  private readonly IMapper _mapper = mapper;

  public async Task<PathDetails> Handle(GetPathDetailsQuery request, CancellationToken cancellationToken)
  {
    var result = await _context.Paths
      .Include(p => p.PathModules.OrderBy(pm => pm.Order))
      .ThenInclude(pm => pm.Module)
      .Where(c => c.Id == request.Id)
      .FirstOrDefaultAsync(cancellationToken);

    if (result == null)
      throw new NotFoundException(nameof(Path), request.Id, NotFoundHelper.PATH_NOT_FOUND);

    //TODO: is there another way to map single item?
    return _mapper.Map<PathDetails>(result);
  }
}