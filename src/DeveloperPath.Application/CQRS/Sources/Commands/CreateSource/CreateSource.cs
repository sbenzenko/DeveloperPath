﻿using AutoMapper;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeveloperPath.Shared.Enums;
using DeveloperPath.Shared.ClientModels;

namespace DeveloperPath.Application.CQRS.Sources.Commands.CreateSource
{
    /// <summary>
    /// Source to create
    /// </summary>
    public record CreateSource : IRequest<Source>
  {
    /// <summary>
    /// Path Id
    /// </summary>
    [Required]
    public int PathId { get; init; }
    /// <summary>
    /// Module Id
    /// </summary>
    [Required]
    public int ModuleId { get; init; }
    /// <summary>
    /// Theme id that the source is for
    /// </summary>
    [Required]
    public int ThemeId { get; init; }
    /// <summary>
    /// Source title
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Title { get; init; }
    /// <summary>
    /// Source short summary
    /// </summary>
    [Required]
    [MaxLength(10000)]
    public string Description { get; init; }
    /// <summary>
    /// Source Url
    /// </summary>
    [Required]
    [MaxLength(500)]
    [RegularExpression(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,4}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)")]
    public string Url { get; init; }
    /// <summary>
    /// Position of source in theme (0-based).
    /// </summary>
    public int Order { get; init; }
    /// <summary>
    /// Type of source (None | Book | Blog | Course | Documentation | QandA | Video)
    /// </summary>
    public SourceType Type { get; init; }
    /// <summary>
    /// Whether the resource Free | Requires registration | Paid only
    /// </summary>
    public Availability Availability { get; init; }
    /// <summary>
    /// Whether inforation is Not applicable (default) | Up-to-date | Somewhat up-to-date | Outdated
    /// </summary>
    public Relevance Relevance { get; init; }
    /// <summary>
    /// List of tags related to theme
    /// </summary>
    public IList<string> Tags { get; init; }
  }

  internal class CreateSourceCommandHandler : IRequestHandler<CreateSource, Source>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateSourceCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<Source> Handle(CreateSource request, CancellationToken cancellationToken)
    {
      //TODO: check if requested module is in requested path (???)
      var path = await _context.Paths.FindAsync(new object[] { request.PathId }, cancellationToken);
      if (path == null)
        throw new NotFoundException(nameof(Path), request.PathId, NotFoundHelper.PATH_NOT_FOUND);

      var theme = await _context.Themes
        .Where(t => t.Id == request.ThemeId && t.ModuleId == request.ModuleId)
        .FirstOrDefaultAsync(cancellationToken);
      if (theme == null)
        throw new NotFoundException(nameof(Theme), request.ThemeId, NotFoundHelper.THEME_NOT_FOUND);

      var entity = new Domain.Entities.Source
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

      return _mapper.Map<Source>(entity);
    }
  }
}
