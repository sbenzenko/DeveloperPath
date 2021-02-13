using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.Sources.Queries.GetSources
{
  public class GetSourceDetailsQuery : IRequest<SourceViewModel>
  {
    public int Id { get; set; }
    public int PathId { get; set; }
    public int ModuleId { get; set; }
    public int ThemeId { get; set; }
  }

  public class GetSourceDetailsQueryHandler : IRequestHandler<GetSourceDetailsQuery, SourceViewModel>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSourceDetailsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<SourceViewModel> Handle(GetSourceDetailsQuery request, CancellationToken cancellationToken)
    {
      //TODO: check if requested module is in requested path (???)
      var path = await _context.Paths.FindAsync(new object[] { request.PathId }, cancellationToken);
      if (path == null)
        throw new NotFoundException(nameof(Path), request.PathId);

      var theme = await _context.Themes
        .Where(t => t.Id == request.ThemeId && t.ModuleId == request.ModuleId)
        .FirstOrDefaultAsync(cancellationToken);
      if (theme == null)
        throw new NotFoundException(nameof(Theme), request.ThemeId);

      var result = await _context.Sources
        .Include(t => t.Theme)
        .Where(t => t.Id == request.Id && t.ThemeId == request.ThemeId)
        .FirstOrDefaultAsync(cancellationToken);

      if (result == null)
        throw new NotFoundException(nameof(Source), request.Id);

      //TODO: is there another way to map single item?
      return _mapper.Map<SourceViewModel>(result);
    }
  }
}
