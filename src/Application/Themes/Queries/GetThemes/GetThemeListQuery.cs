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

namespace DeveloperPath.Application.Themes.Queries.GetThemes
{
  public class GetThemeListQuery : IRequest<IEnumerable<ThemeDto>>
  {
    public int PathId { get; set; }
    public int ModuleId { get; set; }
  }

  public class GetThemesQueryHandler : IRequestHandler<GetThemeListQuery, IEnumerable<ThemeDto>>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetThemesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<IEnumerable<ThemeDto>> Handle(GetThemeListQuery request, CancellationToken cancellationToken)
    {
      return await _context.Themes
        .Where(t => t.ModuleId == request.ModuleId)
        .OrderBy(t => t.Order)
        .ProjectTo<ThemeDto>(_mapper.ConfigurationProvider)
        .ToListAsync(cancellationToken);
    }
  }
}
