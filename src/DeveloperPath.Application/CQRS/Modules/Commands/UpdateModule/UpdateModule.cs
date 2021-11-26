using AutoMapper;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using DeveloperPath.Shared.ClientModels;
using DeveloperPath.Shared.Enums;

namespace DeveloperPath.Application.CQRS.Modules.Commands.UpdateModule
{
    /// <summary>
    /// Module to update
    /// </summary>
    public record UpdateModule : IRequest<Module>
    {
        // TODO: add Prerequisites, provide Order
        /// <summary>
        /// Module Id
        /// </summary>
        [Required]
        public int Id { get; init; }
        /// <summary>
        /// Module title
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
        /// Module short summary
        /// </summary>
        [Required]
        [MaxLength(3000)]
        public string Description { get; init; }
        /// <summary>
        /// Necessity level (Other (default) | Possibilities | Interesting | Good to know | Must know)
        /// </summary>
        public Necessity Necessity { get; init; }
        /// <summary>
        /// Order of module in path (0-based)
        /// </summary>
        public int Order { get; init; }
        /// <summary>
        /// List of tags related to the module
        /// </summary>
        public IList<string> Tags { get; init; }
    }

    internal class UpdateModuleCommandHandler : IRequestHandler<UpdateModule, Module>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateModuleCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Module> Handle(UpdateModule request, CancellationToken cancellationToken)
        {
            var entity = await _context.Modules.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null)
                throw new NotFoundException(nameof(Module), request.Id, NotFoundHelper.MODULE_NOT_FOUND);

            _mapper.Map(request, entity);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<Module>(entity);
        }
    }
}
