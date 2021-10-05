using AutoMapper;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Shared.ClientModels;
using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Application.CQRS.Paths.Commands.CreatePath
{
    /// <summary>
    /// Path to create
    /// </summary>
    public record CreatePath : IRequest<Path>
    {
        /// <summary>
        /// Path title
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Title { get; init; }
        /// <summary>
        /// URI PathKey
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
        /// List of tags related to path
        /// </summary>
        public IList<string> Tags { get; init; }
    }

    internal class CreatePathCommandHandler : IRequestHandler<CreatePath, Path>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreatePathCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Path> Handle(CreatePath request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Path>(request);

            _context.Paths.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<Path>(entity);
        }
    }
}
