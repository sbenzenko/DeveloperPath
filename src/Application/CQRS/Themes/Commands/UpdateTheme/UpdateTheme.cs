using AutoMapper;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Shared.ClientModels;
using DeveloperPath.Domain.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Application.CQRS.Themes.Commands.UpdateTheme
{
  /// <summary>
  /// Theme to update
  /// </summary>
  public partial record UpdateTheme : IRequest<Theme>
  {
    /// <summary>
    /// Theme id
    /// </summary>
    [Required]
    public int Id { get; init; }
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
    /// Theme title
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Title { get; init; }
    /// <summary>
    /// Theme short summary
    /// </summary>
    [Required]
    [MaxLength(3000)]
    public string Description { get; init; }
    /// <summary>
    /// Theme section id (can be null)
    /// </summary>
    public int SectionId { get; init; }
    /// <summary>
    /// Complexity level (Beginner | Intermediate | Advanced)
    /// </summary>
    public Complexity Complexity { get; init; }
    /// <summary>
    /// Necessity level (Other | Possibilities | Interesting | Good to know | Must know)
    /// </summary>
    public Necessity Necessity { get; init; }
    /// <summary>
    /// Position of the theme in module (0-based)
    /// </summary>
    public int Order { get; init; }
    /// <summary>
    /// List of tags related to the theme
    /// </summary>
    public IList<string> Tags { get; set; }
  }

  internal class UpdateThemeCommandHandler : IRequestHandler<UpdateTheme, Theme>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateThemeCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<Theme> Handle(UpdateTheme request, CancellationToken cancellationToken)
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

      Domain.Entities.Section section = null;
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

      return _mapper.Map<Theme>(entity);
    }
  }
}
