using AutoMapper;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Application.Common.Models;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Domain.Enums;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Application.Sources.Commands.CreateSource
{
  public record CreateSourceCommand : IRequest<SourceDto>
  {
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
      var theme = await _context.Themes.FindAsync(new object[] { request.ThemeId }, cancellationToken);
      if (theme == null)
        throw new NotFoundException(nameof(Theme), request.ThemeId);

      var module = await _context.Modules.FindAsync(new object[] { request.ModuleId }, cancellationToken);
      if (module == null)
        throw new NotFoundException(nameof(Module), request.ModuleId);

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
