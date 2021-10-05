using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Shared.ClientModels;
using Shared.ClientModels;

namespace DeveloperPath.Application.CQRS.Modules.Queries.GetModules
{
    /// <summary>
    /// Get module parameters
    /// </summary>
    public class GetModuleListQuery : IRequest<IEnumerable<Module>>
    {
        /// <summary>
        /// URI key
        /// </summary>
        [Required]
        public string PathKey { get; init; }
    }

    internal class GetModulesQueryHandler : IRequestHandler<GetModuleListQuery, IEnumerable<Module>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetModulesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Module>> Handle(GetModuleListQuery request, CancellationToken cancellationToken)
        {
            //TODO: check if requested module is in requested path (???)
            var path = await _context.Paths.FirstOrDefaultAsync(x => x.Key == request.PathKey, cancellationToken: cancellationToken);
            if (path == null)
                throw new NotFoundException(nameof(Path), request.PathKey, NotFoundHelper.PATH_NOT_FOUND);

            // TODO: Order modules (from PathModules.Order)
            return await _context.Paths
                         .AsNoTracking()
                         .Where(p => p.Key == request.PathKey)
                         .SelectMany(p => p.Modules)
                         .Include(m => m.Paths)
                         .Include(m => m.Prerequisites)
                         .ProjectTo<Module>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
        }
    }
}
