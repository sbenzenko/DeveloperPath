using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.Sources.Queries.GetSource
{
  public class GetSourceQuery : IRequest<SourceViewModel>
  {
    public int Id { get; set; }
    public int ModuleId { get; set; }
    public int ThemeId { get; set; }
  }

  public class GetSourceQueryHandler : IRequestHandler<GetSourceQuery, SourceViewModel>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSourceQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<SourceViewModel> Handle(GetSourceQuery request, CancellationToken cancellationToken)
    {
      var module = await _context.Modules.FindAsync(new object[] { request.ModuleId }, cancellationToken);
      if (module == null)
        throw new NotFoundException(nameof(Module), request.ModuleId);

      var theme = await _context.Themes.FindAsync(new object[] { request.ThemeId }, cancellationToken);
      if (theme == null)
        throw new NotFoundException(nameof(Theme), request.ThemeId);

      var result = await _context.Sources
        .Include(t => t.Theme)
        .Where(t => t.Id == request.Id && t.ThemeId == request.ThemeId)
        .FirstOrDefaultAsync(cancellationToken);

      if (result == null)
      {
        throw new NotFoundException(nameof(Source), request.Id);
      }

      //TODO: is there another way to map single item?
      return _mapper.Map<SourceViewModel>(result);
    }
  }
}
