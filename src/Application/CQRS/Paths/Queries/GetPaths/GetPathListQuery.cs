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
        private const string CacheKey = "GetPathsQueryHandler";

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public GetPathsQueryHandler(IApplicationDbContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<IEnumerable<Path>> Handle(GetPathListQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(CacheKey, out IEnumerable<Path> result))
            {
                return result;
            }
            else
            {
                result = await _context.Paths
                    .OrderBy(t => t.Title)
                    .ProjectTo<Path>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
                
                _cache.Set(CacheKey, result, new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
                    SlidingExpiration = TimeSpan.FromSeconds(10)
                });

                return result;
            }
        }
    }
}
