using AutoMapper;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Application.Common.Models;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Application.Themes.Commands.UpdateTheme
{
  public partial record UpdateThemeCommand : IRequest<ThemeDto>
  {
    public int Id { get; init; }
    public int PathId { get; init; }
    public int ModuleId { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public int SectionId { get; init; }
    public ComplexityLevel Complexity { get; init; }
    public NecessityLevel Necessity { get; init; }
    public int Order { get; init; }
  }

  public class UpdateThemeCommandHandler : IRequestHandler<UpdateThemeCommand, ThemeDto>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateThemeCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<ThemeDto> Handle(UpdateThemeCommand request, CancellationToken cancellationToken)
    {
      //TODO: check if requested module is in requested path (???)
      var path = await _context.Paths.FindAsync(new object[] { request.PathId }, cancellationToken);
      if (path == null)
        throw new NotFoundException(nameof(Path), request.PathId);

      var entity = await _context.Themes
        .Where(t => t.Id == request.Id && t.ModuleId == request.ModuleId)
        .FirstOrDefaultAsync(cancellationToken);
      if (entity == null)
        throw new NotFoundException(nameof(Theme), request.Id);

      Section section = null;
      if (request.SectionId > 0)
      {
        section = await _context.Sections
          .Where(s => s.Id == request.SectionId)
          .FirstOrDefaultAsync(cancellationToken);
        if (section == null)
          throw new NotFoundException(nameof(Section), request.SectionId);
      }

      // TODO: is there a way to use init-only fields?
      entity.Title = request.Title;
      entity.Description = request.Description;
      entity.ModuleId = request.ModuleId;
      entity.Section = section;
      entity.Complexity = request.Complexity;
      entity.Necessity = request.Necessity;
      entity.Order = request.Order;

      await _context.SaveChangesAsync(cancellationToken);

      return _mapper.Map<ThemeDto>(entity);
    }
  }
}
