﻿using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Shared.ClientModels;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace DeveloperPath.Application.CQRS.Sources.Queries.GetSources
{
    /// <summary>
    /// Get source parameters
    /// </summary>
    public class GetSourceQuery : IRequest<Source>
  {
    /// <summary>
    /// Source Id
    /// </summary>
    [Required]
    public int Id { get; init; }
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
    /// <summary>
    /// Theme Id
    /// </summary>
    [Required]
    public int ThemeId { get; init; }
  }

  internal class GetSourceQueryHandler : IRequestHandler<GetSourceQuery, Source>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSourceQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<Source> Handle(GetSourceQuery request, CancellationToken cancellationToken)
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

      var result = await _context.Sources
        .Include(t => t.Theme)
        .Where(t => t.Id == request.Id && t.ThemeId == request.ThemeId)
        .FirstOrDefaultAsync(cancellationToken);

      if (result == null)
        throw new NotFoundException(nameof(Source), request.Id, NotFoundHelper.SOURCE_NOT_FOUND);

      //TODO: is there another way to map single item?
      return _mapper.Map<Source>(result);
    }
  }
}
