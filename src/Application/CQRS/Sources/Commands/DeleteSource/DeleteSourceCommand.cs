using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Application.CQRS.Sources.Commands.DeleteSource
{
    /// <summary>
    /// Delete source parameters
    /// </summary>
    public record DeleteSourceCommand : IRequest
    {
        /// <summary>
        /// Source Id
        /// </summary>
        public int Id { get; init; }
        /// <summary>
        /// Path Id
        /// </summary>
        public int PathId { get; init; }
        /// <summary>
        /// Module Id
        /// </summary>
        public int ModuleId { get; init; }
        /// <summary>
        /// Theme Id
        /// </summary>
        public int ThemeId { get; init; }
    }

    internal class DeleteSourceCommandHandler : IRequestHandler<DeleteSourceCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteSourceCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteSourceCommand request, CancellationToken cancellationToken)
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

            var entity = await _context.Sources
              .Where(t => t.Id == request.Id && t.ThemeId == request.ThemeId)
              .FirstOrDefaultAsync(cancellationToken);
            if (entity == null)
                throw new NotFoundException(nameof(Source), request.Id);

            _context.Sources.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
