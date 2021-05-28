using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Shared.ClientModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.ClientModels;

namespace DeveloperPath.Application.CQRS.Modules.Queries.GetModules
{
    /// <summary>
    /// Get module details parameters
    /// </summary>
    public class GetModuleDetailsQuery : IRequest<ModuleDetails>
    {
        /// <summary>
        /// Module Id
        /// </summary>
        [Required]
        public int Id { get; init; }
        /// <summary>
        /// Path Id
        /// </summary>
        [Required]
        public int PathId { get; init; }
    }

    internal class GetModuleDetailsHandler : IRequestHandler<GetModuleDetailsQuery, ModuleDetails>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetModuleDetailsHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ModuleDetails> Handle(GetModuleDetailsQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Modules
              .Include(m => m.Paths)
              .Include(m => m.Prerequisites)
              .Include(m => m.Themes)
              .Include(m => m.Sections)
              .Where(m => m.Id == request.Id)
              .FirstOrDefaultAsync(cancellationToken);

            if (result == null || result.Paths.Where(p => p.Id == request.PathId) == null)
                throw new NotFoundException(nameof(Module), request.Id, NotFoundHelper.MODULE_NOT_FOUND);

            return _mapper.Map<ModuleDetails>(result);
        }
    }
}
