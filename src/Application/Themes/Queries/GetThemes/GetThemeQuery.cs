using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Application.Common.Models;
using DeveloperPath.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.Themes.Queries.GetThemes
{
  public class GetThemeQuery : IRequest<ThemeDto>
  {
    public int Id { get; set; }
    public int PathId { get; set; }
    public int ModuleId { get; set; }
  }

  public class GetThemeQueryHandler : IRequestHandler<GetThemeQuery, ThemeDto>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetThemeQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<ThemeDto> Handle(GetThemeQuery request, CancellationToken cancellationToken)
    {
      var result = await _context.Themes
        .Include(t => t.Prerequisites)
        .Include(t => t.Related)
        .Include(t => t.Section)
        .Where(t => t.Id == request.Id && t.ModuleId == request.ModuleId)
        .FirstOrDefaultAsync(cancellationToken);

      if (result == null)
        throw new NotFoundException(nameof(Theme), request.Id);

      //TODO: is there another way to map single item?
      return _mapper.Map<ThemeDto>(result);
    }
  }
}
