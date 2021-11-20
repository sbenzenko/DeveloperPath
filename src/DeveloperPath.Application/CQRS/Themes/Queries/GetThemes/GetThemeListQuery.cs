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


namespace DeveloperPath.Application.CQRS.Themes.Queries.GetThemes
{
    /// <summary>
    /// Get themes list parameters
    /// </summary>
    public class GetThemeListQuery : IRequest<IEnumerable<Theme>>
  {
    /// <summary>
    /// Path Id
    /// </summary>
    [Required]
    public int PathId { get; init; }
    /// <summary>
    /// Module Id
    /// </summary>
    [Required]
    public int ModuleId { get; init; }
  }

  internal class GetThemeListQueryHandler : IRequestHandler<GetThemeListQuery, IEnumerable<Theme>>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetThemeListQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<IEnumerable<Theme>> Handle(GetThemeListQuery request, CancellationToken cancellationToken)
    {
      //TODO: check if requested module is in requested path (???)
      var path = await _context.Paths.FindAsync(new object[] { request.PathId }, cancellationToken);
      if (path == null)
        throw new NotFoundException(nameof(Path),  request.PathId, NotFoundHelper.PATH_NOT_FOUND);

      return await _context.Themes
        .Where(t => t.ModuleId == request.ModuleId)
        .OrderBy(t => t.Order)
        .ProjectTo<Theme>(_mapper.ConfigurationProvider)
        .ToListAsync(cancellationToken);
    }
  }
}
