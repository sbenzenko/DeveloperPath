using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Shared.Dtos.Models;
using AutoMapper;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.CQRS.Modules.Queries.GetModules
{
    /// <summary>
    /// Get module parameters
    /// </summary>
    public class GetModuleQuery : IRequest<ModuleDto>
    {
        /// <summary>
        /// Module Id
        /// </summary>
        public int Id { get; init; }
        /// <summary>
        /// Path Id
        /// </summary>
        public int PathId { get; init; }
    }

    internal class GetModuleQueryHandler : IRequestHandler<GetModuleQuery, ModuleDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetModuleQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ModuleDto> Handle(GetModuleQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Modules
              .Include(m => m.Paths)
              .Include(m => m.Prerequisites)
              .Where(m => m.Id == request.Id)
              .FirstOrDefaultAsync(cancellationToken);

            if (result == null || result.Paths.Where(p => p.Id == request.PathId) == null)
                throw new NotFoundException(nameof(Module), request.Id);

            //TODO: is there another way to map single item?
            return _mapper.Map<ModuleDto>(result);
        }
    }
}
