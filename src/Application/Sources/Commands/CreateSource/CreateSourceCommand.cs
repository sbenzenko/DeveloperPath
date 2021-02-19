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

namespace DeveloperPath.Application.Sources.Commands.CreateSource
{
  public record CreateSourceCommand : IRequest<SourceDto>
  {
    public int PathId { get; init; }
    public int ModuleId { get; init; }
    public int ThemeId { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public string Url { get; init; }
    public int Order { get; init; }
    public SourceType Type { get; init; }
    public AvailabilityLevel Availability { get; init; }
    public RelevanceLevel Relevance { get; init; }
  }

  public class CreateSourceCommandHandler : IRequestHandler<CreateSourceCommand, SourceDto>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateSourceCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<SourceDto> Handle(CreateSourceCommand request, CancellationToken cancellationToken)
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

      var entity = new Source
      {
        Title = request.Title,
        Description = request.Description,
        Url = request.Url,
        Order = request.Order,
        Type = request.Type,
        Theme = theme,
        Availability = request.Availability,
        Relevance = request.Relevance
      };

      _context.Sources.Add(entity);

      await _context.SaveChangesAsync(cancellationToken);

      return _mapper.Map<SourceDto>(entity);
    }
  }
}
