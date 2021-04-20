using AutoMapper;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Application.Common.Models;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Application.Themes.Commands.UpdateTheme
{
  /// <summary>
  /// Theme to update
  /// </summary>
  public partial record UpdateThemeCommand : IRequest<ThemeDto>
  {
    /// <summary>
    /// Theme id
    /// </summary>
    public int Id { get; init; }
    /// <summary>
    /// Path id
    /// </summary>
    public int PathId { get; init; }
    /// <summary>
    /// Module Id
    /// </summary>
    public int ModuleId { get; init; }
    /// <summary>
    /// Theme title
    /// </summary>
    public string Title { get; init; }
    /// <summary>
    /// Theme short summary
    /// </summary>
    public string Description { get; init; }
    /// <summary>
    /// Theme section id (can be null)
    /// </summary>
    public int SectionId { get; init; }
    /// <summary>
    /// Complexity level (Beginner | Intermediate | Advanced)
    /// </summary>
    public ComplexityLevel Complexity { get; init; }
    /// <summary>
    /// Necessity level (Other | Possibilities | Interesting | Good to know | Must know)
    /// </summary>
    public NecessityLevel Necessity { get; init; }
    /// <summary>
    /// Position of the theme in module (0-based)
    /// </summary>
    public int Order { get; init; }
    /// <summary>
    /// List of tags related to the theme
    /// </summary>
    public IList<string> Tags { get; set; }
  }

  internal class UpdateThemeCommandHandler : IRequestHandler<UpdateThemeCommand, ThemeDto>
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
