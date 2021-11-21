using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Application.Extensions;
using DeveloperPath.Application.Helpers;
using DeveloperPath.Shared.ClientModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
 

namespace DeveloperPath.Application.CQRS.Paths.Queries.GetPaths
{
    /// <summary>
    /// Get deleted paths
    /// </summary>
    public class GetDeletedPathListQuery : IRequest<PagedList<DeletedPath>>
    {
        /// <summary>
        /// Page Size (5 10 25 50 100)
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        ///  Page Number
        /// </summary>
        public int PageNumber { get; set; }
    }

    internal class GetDeletedPathListQueryHandler : IRequestHandler<GetDeletedPathListQuery, PagedList<DeletedPath>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetDeletedPathListQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<PagedList<DeletedPath>> Handle(GetDeletedPathListQuery request, CancellationToken cancellationToken)
        {
            return _context.Paths
                .OrderBy(t => t.Title)
                .IgnoreQueryFilters()
                .Where(x=>x.Deleted != null)
                .ProjectTo<DeletedPath>(_mapper.ConfigurationProvider)
                .ToPagedListAsync(request.PageSize, request.PageNumber, cancellationToken);
        }
    }
}
