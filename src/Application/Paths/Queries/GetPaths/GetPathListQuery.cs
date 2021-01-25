using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.Paths.Queries.GetPaths
{
  public class GetPathListQuery : IRequest<IEnumerable<PathDto>>
  {
  }

  public class GetPathsQueryHandler : IRequestHandler<GetPathListQuery, IEnumerable<PathDto>>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPathsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<IEnumerable<PathDto>> Handle(GetPathListQuery request, CancellationToken cancellationToken)
    {
      return await _context.Paths
              .ProjectTo<PathDto>(_mapper.ConfigurationProvider)
              .OrderBy(t => t.Title)
              .ToListAsync(cancellationToken);
    }
  }
}
