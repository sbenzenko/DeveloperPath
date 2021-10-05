using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DeveloperPath.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.ClientModels;

namespace DeveloperPath.Application.CQRS.Modules.Queries.GetModules
{
    /// <summary>
    /// Get list of visible modules
    /// </summary>
    public class GetAllAvailableModulesQuery : IRequest<List<ModuleTitle>>
    {
        
    }

    internal class GetAllAvailableModulesQueryHandler 
        : IRequestHandler<GetAllAvailableModulesQuery, List<ModuleTitle>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;


        public GetAllAvailableModulesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Task<List<ModuleTitle>> Handle(GetAllAvailableModulesQuery request, CancellationToken cancellationToken)
        {
            return _context.Modules
                .ProjectTo<ModuleTitle>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
