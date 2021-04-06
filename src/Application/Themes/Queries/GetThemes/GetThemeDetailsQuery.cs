﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.Themes.Queries.GetThemes
{
  public class GetThemeDetailsQuery : IRequest<ThemeViewModel>
  {
    public int Id { get; set; }
    public int PathId { get; set; }
    public int ModuleId { get; set; }
  }

  public class GetThemeDetailsQueryHandler : IRequestHandler<GetThemeDetailsQuery, ThemeViewModel>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetThemeDetailsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<ThemeViewModel> Handle(GetThemeDetailsQuery request, CancellationToken cancellationToken)
    {
      //TODO: check if requested module is in requested path (???)
      var path = await _context.Paths.FindAsync(new object[] { request.PathId }, cancellationToken);
      if (path == null)
        throw new NotFoundException(nameof(Path), request.PathId);

      var result = await _context.Themes
        .Include(t => t.Prerequisites)
        .Include(t => t.Related)
        .Include(t => t.Module)
        .Include(t => t.Section)
        .Include(t => t.Sources)
        .Where(t => t.Id == request.Id && t.ModuleId == request.ModuleId)
        .FirstOrDefaultAsync(cancellationToken);

      if (result == null)
        throw new NotFoundException(nameof(Theme), request.Id);

      //TODO: is there another way to map single item?
      return _mapper.Map<ThemeViewModel>(result);
    }
  }
}
