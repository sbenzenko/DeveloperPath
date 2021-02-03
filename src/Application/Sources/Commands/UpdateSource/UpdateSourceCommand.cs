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

namespace DeveloperPath.Application.Sources.Commands.UpdateSource
{
  public partial record UpdateSourceCommand : IRequest<SourceDto>
  {
    public int Id { get; init; }
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

  public class UpdateSourceCommandHandler : IRequestHandler<UpdateSourceCommand, SourceDto>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateSourceCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<SourceDto> Handle(UpdateSourceCommand request, CancellationToken cancellationToken)
    {
      var entity = await _context.Sources.FindAsync(new object[] { request.Id }, cancellationToken);
      if (entity == null)
        throw new NotFoundException(nameof(Source), request.Id);

      var theme = await _context.Themes.FindAsync(new object[] { request.ThemeId }, cancellationToken);
      if (theme == null)
        throw new NotFoundException(nameof(Theme), request.ThemeId);

      var module = await _context.Modules.FindAsync(new object[] { request.ModuleId }, cancellationToken);
      if (module == null)
        throw new NotFoundException(nameof(Module), request.ModuleId);

      // TODO: is there a way to use init-only fields?
      entity.ThemeId = request.ThemeId;
      entity.Title = request.Title;
      entity.Description = request.Description;
      entity.Url = request.Url;
      entity.Order = request.Order;
      entity.Type = request.Type;
      entity.Availability = request.Availability;
      entity.Relevance = request.Relevance;

      await _context.SaveChangesAsync(cancellationToken);

      return _mapper.Map<SourceDto>(entity);
    }
  }
}
