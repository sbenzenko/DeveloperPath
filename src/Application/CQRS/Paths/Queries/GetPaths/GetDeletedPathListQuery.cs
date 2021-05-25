using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DeveloperPath.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DeveloperPath.Domain.Shared.ClientModels;

namespace DeveloperPath.Application.CQRS.Paths.Queries.GetPaths
{
    /// <summary>
    /// Get deleted paths
    /// </summary>
    public class GetDeletedPathListQuery : IRequest<IEnumerable<DeletedPath>>
    {
    }

    internal class GetDeletedPathListQueryHandler : IRequestHandler<GetDeletedPathListQuery, IEnumerable<DeletedPath>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetDeletedPathListQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DeletedPath>> Handle(GetDeletedPathListQuery request, CancellationToken cancellationToken)
        {
            return await _context.Paths
                .OrderBy(t => t.Title)
                .IgnoreQueryFilters()
                .Where(x=>x.Deleted != null)
                .ProjectTo<DeletedPath>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
