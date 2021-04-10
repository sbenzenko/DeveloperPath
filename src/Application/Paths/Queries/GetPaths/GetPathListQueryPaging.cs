using AutoMapper;
using AutoMapper.QueryableExtensions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Application.Common.Models;
using DeveloperPath.Application.Paging;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Application.Paths.Queries.GetPaths
{
    public class GetPathListQueryPaging : IRequest<(PaginationData,IEnumerable<PathDto>)>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetPathsPagingQueryHandler : IRequestHandler<GetPathListQueryPaging, (PaginationData,IEnumerable<PathDto>)>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPathsPagingQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(PaginationData, IEnumerable<PathDto>)> Handle(GetPathListQueryPaging request, CancellationToken cancellationToken)
        {
            IEnumerable<PathDto> pathCollection = null;

            if (request.PageNumber > 0 || request.PageSize > 0)
            {
                pathCollection = await _context.Paths.OrderBy(t => t.Title)
         .ProjectTo<PathDto>(_mapper.ConfigurationProvider).Skip((request.PageNumber - 1) * request.PageSize)
         .Take(request.PageSize)
         .ToListAsync(cancellationToken);

                return (new PaginationData(request.PageNumber, request.PageSize), pathCollection);
            }


            pathCollection = await _context.Paths
         .OrderBy(t => t.Title)
         .ProjectTo<PathDto>(_mapper.ConfigurationProvider)
         .ToListAsync(cancellationToken);
            return (new PaginationData(request.PageNumber, request.PageSize), pathCollection);
        }
    }
}
