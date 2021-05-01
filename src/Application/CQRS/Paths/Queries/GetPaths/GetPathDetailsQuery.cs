using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Shared.ClientModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.ClientModels;

namespace DeveloperPath.Application.CQRS.Paths.Queries.GetPaths
{
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

  internal class GetPathDetailsQueryHandler : IRequestHandler<GetPathDetailsQuery, PathDetails>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPathDetailsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<PathDetails> Handle(GetPathDetailsQuery request, CancellationToken cancellationToken)
    {
      var result = await _context.Paths
        .Include(p => p.Modules)
        .Where(c => c.Id == request.Id)
        .FirstOrDefaultAsync(cancellationToken);

      if (result == null)
      {
        throw new NotFoundException(nameof(Path), request.Id);
      }

      //TODO: is there another way to map single item?
      return _mapper.Map<PathDetails>(result);
    }
  }
}
