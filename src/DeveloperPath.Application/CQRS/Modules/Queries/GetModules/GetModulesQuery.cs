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
using DeveloperPath.Shared.ClientModels;
using System;

namespace DeveloperPath.Application.CQRS.Modules.Queries.GetModules
{
    /// <summary>
    /// Get module parameters
    /// </summary>
    public class GetModulesQuery : IRequest<IEnumerable<Module>>
    {
        /// <summary>
        /// PathKey
        /// </summary>
        [Required]
        public string PathKey { get; init; }
    }

    internal class GetModulesQueryHandler : IRequestHandler<GetModulesQuery, IEnumerable<Module>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetModulesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Module>> Handle(GetModulesQuery request, CancellationToken cancellationToken)
        {
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
            // return await _context.Paths
            //              .AsNoTracking()
            //              .Include(p => p.ModulesLink
            //                  .OrderBy(mp => mp.Order))
            //              .ThenInclude(m => m.Module)
            //              .ThenInclude(m => m.Prerequisites)
            //              .ProjectTo<Module>(_mapper.ConfigurationProvider)
            //              .ToListAsync(cancellationToken);
        }
    }
}
