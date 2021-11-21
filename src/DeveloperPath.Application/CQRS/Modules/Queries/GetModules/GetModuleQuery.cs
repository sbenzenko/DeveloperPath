using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Shared.ClientModels;

namespace DeveloperPath.Application.CQRS.Modules.Queries.GetModules
{
    /// <summary>
    /// Get module parameters
    /// </summary>
    public class GetModuleQuery : IRequest<Module>
  {
    /// <summary>
    /// Module Id
    /// </summary>
    [Required]
    public int Id { get; init; }
    /// <summary>
    /// Path URI key
    /// </summary>
    [Required]
    public string PathKey { get; init; }
  }

  internal class GetModuleQueryHandler : IRequestHandler<GetModuleQuery, Module>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetModuleQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<Module> Handle(GetModuleQuery request, CancellationToken cancellationToken)
    {
      var result = await _context.Modules
        .Include(m => m.Paths)
        .Include(m => m.Prerequisites)
        .Where(m => m.Id == request.Id)
        .FirstOrDefaultAsync(cancellationToken);

      if (result == null || result.Paths.Where(p => p.Key == request.PathKey) == null)
        throw new NotFoundException(nameof(Module), request.Id, NotFoundHelper.MODULE_NOT_FOUND);

      //TODO: is there another way to map single item?
      return _mapper.Map<Module>(result);
    }
  }
}
