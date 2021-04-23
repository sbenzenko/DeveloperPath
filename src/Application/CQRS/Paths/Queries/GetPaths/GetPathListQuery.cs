using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Shared.Dtos.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DeveloperPath.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.CQRS.Paths.Queries.GetPaths
{
    /// <summary>
    /// Get path
    /// </summary>
    public class GetPathListQuery : IRequest<IEnumerable<PathDto>>
    {
    }

    internal class GetPathsQueryHandler : IRequestHandler<GetPathListQuery, IEnumerable<PathDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPathsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PathDto>> Handle(GetPathListQuery request, CancellationToken cancellationToken)
        {
            return await _context.Paths
              .OrderBy(t => t.Title)
              .ProjectTo<PathDto>(_mapper.ConfigurationProvider)
              .ToListAsync(cancellationToken);
        }
    }
}
