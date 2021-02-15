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

namespace DeveloperPath.Application.Themes.Queries.GetThemes
{
  public class GetThemeListQuery : IRequest<IEnumerable<ThemeDto>>
  {
    public int PathId { get; set; }
    public int ModuleId { get; set; }
  }

  public class GetThemeListQueryHandler : IRequestHandler<GetThemeListQuery, IEnumerable<ThemeDto>>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetThemeListQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<IEnumerable<ThemeDto>> Handle(GetThemeListQuery request, CancellationToken cancellationToken)
    {
      var path = await _context.Paths
        .Where(c => c.Id == request.PathId)
        .FirstOrDefaultAsync(cancellationToken);
      if (path == null)
        throw new NotFoundException(nameof(Path), request.PathId);

      return await _context.Themes
        .Where(t => t.ModuleId == request.ModuleId)
        .OrderBy(t => t.Order)
        .ProjectTo<ThemeDto>(_mapper.ConfigurationProvider)
        .ToListAsync(cancellationToken);
    }
  }
}
