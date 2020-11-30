using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.Paths.Queries.GetPaths
{
  public class GetPathQuery : IRequest<PathDetailsDto>
  {
    public int Id { get; set; }
  }

  public class GetPathQueryHandler : IRequestHandler<GetPathQuery, PathDetailsDto>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPathQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<PathDetailsDto> Handle(GetPathQuery request, CancellationToken cancellationToken)
    {
      var result = await _context.Paths
        .Include(p => p.Modules)
        .Where(c => c.Id == request.Id)
        .FirstOrDefaultAsync();

      if (result == null)
      {
        throw new NotFoundException(nameof(Paths), request.Id);
      }

      //TODO: is there another way to map single item?
      return _mapper.Map<PathDetailsDto>(result);
    }
  }
}
