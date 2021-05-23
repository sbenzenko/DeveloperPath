using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Shared.ClientModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.ClientModels;


namespace DeveloperPath.Application.CQRS.Sources.Queries.GetSources
{
  /// <summary>
  /// Get sources list parameters
  /// </summary>
  public class GetSourceListQuery : IRequest<IEnumerable<Source>>
  {
    /// <summary>
    /// Path id
    /// </summary>
    [Required]
    public int PathId { get; init; }
    /// <summary>
    /// Module Id
    /// </summary>
    [Required]
    public int ModuleId { get; init; }
    /// <summary>
    /// Theme Id
    /// </summary>
    [Required]
    public int ThemeId { get; init; }
  }

  internal class GetSourceListQueryHandler : IRequestHandler<GetSourceListQuery, IEnumerable<Source>>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSourceListQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<IEnumerable<Source>> Handle(GetSourceListQuery request, CancellationToken cancellationToken)
    {
      //TODO: check if requested module is in requested path (???)
      var path = await _context.Paths.FindAsync(new object[] { request.PathId }, cancellationToken);
      if (path == null)
        throw new NotFoundException(nameof(Path), request.PathId, NotFoundHelper.PATH_NOT_FOUND);

      var theme = await _context.Themes
        .Where(t => t.Id == request.ThemeId && t.ModuleId == request.ModuleId)
        .FirstOrDefaultAsync(cancellationToken);
      if (theme == null)
        throw new NotFoundException(nameof(Theme), request.ThemeId, NotFoundHelper.THEME_NOT_FOUND);

      return await _context.Sources
        .Where(s => s.ThemeId == request.ThemeId)
        .OrderBy(s => s.Order)
        .ProjectTo<Source>(_mapper.ConfigurationProvider)
        .ToListAsync(cancellationToken);
    }
  }
}
