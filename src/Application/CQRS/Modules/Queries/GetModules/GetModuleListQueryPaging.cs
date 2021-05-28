using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Application.Common.Models;
using DeveloperPath.Domain.Shared.ClientModels;
using Shared.ClientModels;

namespace DeveloperPath.Application.CQRS.Modules.Queries.GetModules
{
  /// <summary>
  /// Get modules paged
  /// </summary>
  public class GetModuleListQueryPaging : IRequest<(PaginationData, IEnumerable<Module>)>
  {
    /// <summary>
    /// Path id
    /// </summary>
    [Required]
    public string PathKey { get; init; }
    /// <summary>
    /// Page number
    /// </summary>
    public int PageNumber { get; init; }
    /// <summary>
    /// Items per page
    /// </summary>
    public int PageSize { get; init; }
  }


  internal class GetModulesQueryPagingHandler : IRequestHandler<GetModuleListQueryPaging, (PaginationData, IEnumerable<Module>)>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetModulesQueryPagingHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<(PaginationData, IEnumerable<Module>)> Handle(GetModuleListQueryPaging request, CancellationToken cancellationToken)
    {
      //TODO: check if requested module is in requested path (???)
      var path = await _context.Paths.FirstOrDefaultAsync(x => x.Key == request.PathKey, cancellationToken: cancellationToken);
      if (path == null)
        throw new NotFoundException(nameof(Path), request.PathKey, NotFoundHelper.PATH_NOT_FOUND);
      IEnumerable<Module> modules = null;

      if (request.PageNumber > 0 || request.PageSize > 0)
      {
        modules = await _context.Paths
           .Where(p => p.Key == request.PathKey)
           .SelectMany(p => p.Modules)
           .Include(m => m.Paths)
           .Include(m => m.Prerequisites)
           .ProjectTo<Module>(_mapper.ConfigurationProvider).Skip((request.PageNumber - 1) * request.PageSize)
           .Take(request.PageSize)
           .ToListAsync(cancellationToken);
        return (new PaginationData(request.PageNumber, request.PageSize), modules);
      }

      // TODO: Order modules (from PathModules.Order)
      modules = await _context.Paths
          .Where(p => p.Key == request.PathKey)
          .SelectMany(p => p.Modules)
          .Include(m => m.Paths)
          .Include(m => m.Prerequisites)
          .ProjectTo<Module>(_mapper.ConfigurationProvider)
          .ToListAsync(cancellationToken);
      return (new PaginationData(request.PageNumber, request.PageSize), modules);
    }
  }

}
