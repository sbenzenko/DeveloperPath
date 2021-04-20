﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Application.Common.Models;
using DeveloperPath.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.Paths.Queries.GetPaths
{
  /// <summary>
  /// Get path parameters
  /// </summary>
  public class GetPathQuery : IRequest<PathDto>
  {
    /// <summary>
    /// Path id
    /// </summary>
    public int Id { get; init; }
  }

  internal class GetPathQueryHandler : IRequestHandler<GetPathQuery, PathDto>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPathQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<PathDto> Handle(GetPathQuery request, CancellationToken cancellationToken)
    {
      var result = await _context.Paths
        .Where(c => c.Id == request.Id)
        .FirstOrDefaultAsync(cancellationToken);

      if (result == null)
        throw new NotFoundException(nameof(Path), request.Id);

      //TODO: is there another way to map single item?
      return _mapper.Map<PathDto>(result);
    }
  }
}
