using AutoMapper;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Shared.ClientModels;
using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;



namespace DeveloperPath.Application.CQRS.Paths.Commands.UpdatePath
{
    /// <summary>
    /// Path to update
    /// </summary>
    public record UpdatePath : IRequest<Path>
    {
        /// <summary>
        /// Id of the path to update
        /// </summary>
        [Required]
        public int Id { get; init; }
        /// <summary>
        /// Path title
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Title { get; init; }

        /// <summary>
        /// URI Key
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Key { get; init; }

        /// <summary>
        /// Path short summary
        /// </summary>
        [Required]
        [MaxLength(3000)]
        public string Description { get; init; }
        /// <summary>
        /// Make Path visible for users
        /// </summary>
        [Required]
        public bool IsVisible { get; init; }
        /// <summary>
        /// List of tags related to path
        /// </summary>
        public IList<string> Tags { get; init; }
    }

    internal class UpdatePathCommandHandler : IRequestHandler<UpdatePath, Path>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdatePathCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Path> Handle(UpdatePath request, CancellationToken cancellationToken)
        {
            var entity = await _context.Paths.FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Path), request.Id, NotFoundHelper.PATH_NOT_FOUND);
            }

            _mapper.Map(request, entity);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<Path>(entity);
        }
    }
}
