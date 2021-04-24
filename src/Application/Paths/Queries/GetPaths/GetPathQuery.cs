using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.Paths.Queries.GetPaths
{
  /// <summary>
  /// Get path parameters
  /// </summary>
  public class GetPathQuery : IRequest<Path>
  {
    /// <summary>
    /// Path id
    /// </summary>
    [Required]
    public int Id { get; init; }
  }

  internal class GetPathQueryHandler : IRequestHandler<GetPathQuery, Path>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPathQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<Path> Handle(GetPathQuery request, CancellationToken cancellationToken)
    {
      var result = await _context.Paths
        .Where(c => c.Id == request.Id)
        .FirstOrDefaultAsync(cancellationToken);

      if (result == null)
        throw new NotFoundException(nameof(Path), request.Id);

      //TODO: is there another way to map single item?
      return _mapper.Map<Path>(result);
    }
  }
}
