using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Application.Common.Models;
using DeveloperPath.Application.Paging;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.Modules.Queries.GetModules
{
  /// <summary>
  /// Get modules paged
  /// </summary>
  public class GetModuleListQueryPaging : IRequest<(PaginationData, IEnumerable<ModuleDto>)>
  {
    /// <summary>
    /// Path id
    /// </summary>
    [Required]
    public int PathId { get; init; }
    /// <summary>
    /// Page number
    /// </summary>
    public int PageNumber { get; init; }
    /// <summary>
    /// Items per page
    /// </summary>
    public int PageSize { get; init; }
  }


  internal class GetModulesQueryPagingHandler : IRequestHandler<GetModuleListQueryPaging, (PaginationData, IEnumerable<ModuleDto>)>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetModulesQueryPagingHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<(PaginationData, IEnumerable<ModuleDto>)> Handle(GetModuleListQueryPaging request, CancellationToken cancellationToken)
    {
      //TODO: check if requested module is in requested path (???)
      var path = await _context.Paths.FindAsync(new object[] { request.PathId }, cancellationToken);
      if (path == null)
        throw new NotFoundException(nameof(Path), request.PathId);
      IEnumerable<ModuleDto> modules = null;

      if (request.PageNumber > 0 || request.PageSize > 0)
      {
        modules = await _context.Paths
           .Where(p => p.Id == request.PathId)
           .SelectMany(p => p.Modules)
           .Include(m => m.Paths)
           .Include(m => m.Prerequisites)
           .ProjectTo<ModuleDto>(_mapper.ConfigurationProvider).Skip((request.PageNumber - 1) * request.PageSize)
           .Take(request.PageSize)
           .ToListAsync(cancellationToken);
        return (new PaginationData(request.PageNumber, request.PageSize), modules);
      }

      // TODO: Order modules (from PathModules.Order)
      modules = await _context.Paths
          .Where(p => p.Id == request.PathId)
          .SelectMany(p => p.Modules)
          .Include(m => m.Paths)
          .Include(m => m.Prerequisites)
          .ProjectTo<ModuleDto>(_mapper.ConfigurationProvider)
          .ToListAsync(cancellationToken);
      return (new PaginationData(request.PageNumber, request.PageSize), modules);
    }
  }

}
