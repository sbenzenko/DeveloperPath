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

namespace DeveloperPath.Application.Themes.Commands.CreateTheme
{

  /// <summary>
  /// Represents developer theme entity
  /// </summary>
  public record CreateThemeCommand : IRequest<ThemeDto>
  {
    public int PathId { get; init; }
    public int ModuleId { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public int SectionId { get; init; }
    public ComplexityLevel Complexity { get; init; }
    public NecessityLevel Necessity { get; init; }
    public int Order { get; init; }
  }

  public class CreateThemeCommandHandler : IRequestHandler<CreateThemeCommand, ThemeDto>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateThemeCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<ThemeDto> Handle(CreateThemeCommand request, CancellationToken cancellationToken)
    {
      //TODO: check if requested module is in requested path (???)
      var path = await _context.Paths.FindAsync(new object[] { request.PathId }, cancellationToken);
      if (path == null)
        throw new NotFoundException(nameof(Path), request.PathId);

      var module = await _context.Modules.FindAsync(new object[] { request.ModuleId }, cancellationToken);
      if (module == null)
        throw new NotFoundException(nameof(Module), request.ModuleId);

      Section section = null;
      if (request.SectionId > 0)
      {
        section = await _context.Sections
          .Where(s => s.Id == request.SectionId)
          .FirstOrDefaultAsync(cancellationToken);

        if (section == null)
          throw new NotFoundException(nameof(Section), request.SectionId);
      }

      var entity = new Theme
      {
        Title = request.Title,
        Description = request.Description,
        Complexity = request.Complexity,
        Necessity = request.Necessity,
        ModuleId = request.ModuleId,
        Module = module,
        Section = section,
        Order = request.Order
      };

      _context.Themes.Add(entity);

      await _context.SaveChangesAsync(cancellationToken);

      return _mapper.Map<ThemeDto>(entity);
    }
  }
}
