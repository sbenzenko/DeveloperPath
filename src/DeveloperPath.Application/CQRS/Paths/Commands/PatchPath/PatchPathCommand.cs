using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Shared.ClientModels;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.CQRS.Paths.Commands.PatchPath
{
    /// <summary>
    /// Patch command for updating the Path
    /// </summary>
    public class PatchPathCommand : IRequest<Path>
    {
        private readonly int _pathId;
        private readonly JsonPatchDocument _patchDocument;
        private readonly bool _shouldIgnoreDeletedItems;

        /// <summary>
        /// Creates the command
        /// </summary>
        /// <param name="pathId">Id of Path</param>
        /// <param name="patchDocument">Json Patch document</param>
        /// <param name="shouldIgnoreDeletedItems">The flag allows to get deleted items while ignoring the query filters</param>

        public PatchPathCommand(int pathId, JsonPatchDocument patchDocument, bool shouldIgnoreDeletedItems = true)
        {
            _pathId = pathId;
            _patchDocument = patchDocument;
            _shouldIgnoreDeletedItems = shouldIgnoreDeletedItems;
        }

        /// <summary>
        /// Id of Path
        /// </summary>
        public int PathId => _pathId;
        /// <summary>
        /// Special Json Document for Patch feature
        /// </summary>
        public JsonPatchDocument PatchDocument => _patchDocument;

        /// <summary>
        /// The flag allows to get deleted items while ignoring the query filters
        /// </summary>
        public bool ShouldIgnoreDeletedItems => _shouldIgnoreDeletedItems;
    }

    /// <summary>
    /// Patch path command handler
    /// </summary>
    internal class PatchPathCommandHandler : IRequestHandler<PatchPathCommand, Path>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PatchPathCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Path> Handle(PatchPathCommand request, CancellationToken cancellationToken)
        {
            var query = _context.Paths.AsQueryable();
            if (!request.ShouldIgnoreDeletedItems)
            {
                query = query.IgnoreQueryFilters();
            }
            var path = await query
                .FirstOrDefaultAsync(x => x.Id == request.PathId, cancellationToken: cancellationToken);

            if (path == null)
                throw new NotFoundException(nameof(Path), request.PathId, NotFoundHelper.PATH_NOT_FOUND);

            request.PatchDocument.ApplyTo(path);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<Path>(path);
        }
    }
}