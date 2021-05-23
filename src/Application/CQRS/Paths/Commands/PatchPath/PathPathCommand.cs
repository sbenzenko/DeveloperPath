using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Shared.ClientModels;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Shared.ClientModels;

namespace DeveloperPath.Application.CQRS.Paths.Commands.PatchPath
{
    /// <summary>
    /// Patch command for updating the Path
    /// </summary>
    public class PathPathCommand : IRequest<Path>
    {
        private readonly int _pathId;
        private readonly JsonPatchDocument _patchDocument;

        /// <summary>
        /// Creates the command
        /// </summary>
        /// <param name="pathId">Id of Path</param>
        /// <param name="patchDocument">Json Patch document</param>
        public PathPathCommand(int pathId, JsonPatchDocument patchDocument)
        {
            _pathId = pathId;
            _patchDocument = patchDocument;
        }

        /// <summary>
        /// Id of Path
        /// </summary>
        public int PathId => _pathId;
        /// <summary>
        /// Special Json Document for Patch feature
        /// </summary>
        public JsonPatchDocument PatchDocument => _patchDocument;
    }

    /// <summary>
    /// Patch path command handler
    /// </summary>
    public class PatchPathCommandHandler : IRequestHandler<PathPathCommand, Path>
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
        public async Task<Path> Handle(PathPathCommand request, CancellationToken cancellationToken)
        {
            var path = await _context.Paths.FirstOrDefaultAsync(x => x.Id == request.PathId, cancellationToken: cancellationToken);
            if (path == null)
                throw new NotFoundException(nameof(Path), request.PathId, NotFoundHelper.PATH_NOT_FOUND);
            request.PatchDocument.ApplyTo(path);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<Path>(path);
        }
    }
}