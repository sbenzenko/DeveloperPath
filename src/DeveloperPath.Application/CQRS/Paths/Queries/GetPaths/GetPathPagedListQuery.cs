using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Shared.ClientModels;
using DeveloperPath.Application.Extensions;
using DeveloperPath.Application.Helpers;

namespace DeveloperPath.Application.CQRS.Paths.Queries.GetPaths
{
    /// <summary>
    /// Get paths paged
    /// </summary>
    public class GetPathPagedListQuery : IRequest<PagedList<Path>>
    {
        /// <summary>
        /// Page number
        /// </summary>
        public int PageNumber { get; init; }
        /// <summary>
        /// Items per page
        /// </summary>
        public int PageSize { get; init; }
        public bool OnlyVisible { get; set; }
    }

    internal class GetPathsPagingQueryHandler : IRequestHandler<GetPathPagedListQuery, PagedList<Path>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPathsPagingQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedList<Path>> Handle(GetPathPagedListQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Paths.OrderBy(t => t.Title).AsQueryable();

            if (request.OnlyVisible)
            {
                query = query.Where(x => x.IsVisible);
            }

            var mappedQuery = query.ProjectTo<Path>(_mapper.ConfigurationProvider);

            PagedList<Path> pathCollection = default;

            pathCollection = await mappedQuery.ToPagedListAsync(request.PageNumber, request.PageSize, cancellationToken);
            return pathCollection;
        }
    }
}
