using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Shared.ClientModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.CQRS.Modules.Queries.GetModules
{
    /// <summary>
    /// Get list of visible modules
    /// </summary>
    public class GetAllAvailableModulesQuery : IRequest<List<ModuleDetails>>
    {
        
    }

    internal class GetAllAvailableModulesQueryHandler 
        : IRequestHandler<GetAllAvailableModulesQuery, List<ModuleDetails>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;


        public GetAllAvailableModulesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Task<List<ModuleDetails>> Handle(GetAllAvailableModulesQuery request, CancellationToken cancellationToken)
        {
            return _context.Modules
                .Include(x=>x.Prerequisites)
                .ProjectTo<ModuleDetails>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
