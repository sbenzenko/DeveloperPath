using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.Modules.Queries.GetModules
{
  public class GetModuleQuery : IRequest<ModuleViewModel>
  {
    public int Id { get; set; }
  }

  public class GetModuleQueryHandler : IRequestHandler<GetModuleQuery, ModuleViewModel>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetModuleQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<ModuleViewModel> Handle(GetModuleQuery request, CancellationToken cancellationToken)
    {
      var result = await _context.Modules
        .Include(m => m.Paths)
        .Include(m => m.Prerequisites)
        .Include(m => m.Themes)
        .Include(m => m.Sections)
        .Where(m => m.Id == request.Id)
        .FirstOrDefaultAsync(cancellationToken);

      if (result == null)
      {
        throw new NotFoundException(nameof(Module), request.Id);
      }

      //TODO: is there another way to map single item?
      return _mapper.Map<ModuleViewModel>(result);
    }
  }
}
