using AutoMapper;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Application.Common.Models;
using DeveloperPath.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Application.Paths.Commands.UpdatePath
{

  /// <summary>
  /// Represents developer path entity
  /// </summary>
  public partial record UpdatePathCommand : IRequest<PathDto>
  {
    /// <summary>
    /// Id of the path to update
    /// </summary>
    public int Id { get; init; }
    /// <summary>
    /// Path name
    /// </summary>
    public string Title { get; init; }
    /// <summary>
    /// Path short summary
    /// </summary>
    public string Description { get; init; }
    /// <summary>
    /// List of tags related to path.
    /// Use generalized tags, e.g.:
    ///  - Path ASP.NET - Tags: #Development #Web
    ///  - Path Unity - Tags: #Development #Games
    /// </summary>
    public IList<string> Tags { get; init; }
  }

  public class UpdatePathCommandHandler : IRequestHandler<UpdatePathCommand, PathDto>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdatePathCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<PathDto> Handle(UpdatePathCommand request, CancellationToken cancellationToken)
    {
      var entity = await _context.Paths.FindAsync(new object[] { request.Id }, cancellationToken);

      if (entity == null)
      {
        throw new NotFoundException(nameof(Path), request.Id);
      }

      // TODO: is there a way to use init-only fields?
      entity.Title = request.Title;
      entity.Description = request.Description;
      entity.Tags = request.Tags;

      await _context.SaveChangesAsync(cancellationToken);

      return _mapper.Map<PathDto>(entity);
    }
  }
}
