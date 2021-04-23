using Application.Shared.Dtos.Models;
using AutoMapper;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Entities;
using Domain.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Application.CQRS.Sources.Commands.CreateSource
{
    /// <summary>
    /// Source to create
    /// </summary>
    public record CreateSourceCommand : IRequest<SourceDto>
    {
        /// <summary>
        /// Path Id
        /// </summary>
        public int PathId { get; init; }
        /// <summary>
        /// Module Id
        /// </summary>
        public int ModuleId { get; init; }
        /// <summary>
        /// Theme id that the source is for
        /// </summary>
        public int ThemeId { get; init; }
        /// <summary>
        /// Source title
        /// </summary>
        public string Title { get; init; }
        /// <summary>
        /// Source short summary
        /// </summary>
        public string Description { get; init; }
        /// <summary>
        /// Source Url
        /// </summary>
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
        public AvailabilityLevel Availability { get; init; }
        /// <summary>
        /// Whether inforation is Not applicable (default) | Up-to-date | Somewhat up-to-date | Outdated
        /// </summary>
        public RelevanceLevel Relevance { get; init; }
        /// <summary>
        /// List of tags related to theme
        /// </summary>
        public IList<string> Tags { get; init; }
    }

    internal class CreateSourceCommandHandler : IRequestHandler<CreateSourceCommand, SourceDto>
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
