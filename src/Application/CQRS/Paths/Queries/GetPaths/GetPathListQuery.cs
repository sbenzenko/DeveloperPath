using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Shared.ClientModels;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace DeveloperPath.Application.CQRS.Paths.Queries.GetPaths
{
    /// <summary>
    /// Get path
    /// </summary>
    public class GetPathListQuery : IRequest<IEnumerable<Path>>
    {
    }

    internal class GetPathsQueryHandler : IRequestHandler<GetPathListQuery, IEnumerable<Path>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPathsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<Path>> Handle(GetPathListQuery request, CancellationToken cancellationToken)
        {
            return await _context.Paths
                .OrderBy(t => t.Title)
                .ProjectTo<Path>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken); ;
        }
    }
}
