using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Application.Paths.Commands.CreatePath
{

  /// <summary>
  /// Represents developer path entity
  /// </summary>
  public record CreatePathCommand : IRequest<int>
  {
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

  public class CreatePathCommandHandler : IRequestHandler<CreatePathCommand, int>
  {
    private readonly IApplicationDbContext _context;

    public CreatePathCommandHandler(IApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<int> Handle(CreatePathCommand request, CancellationToken cancellationToken)
    {
      var entity = new Path
      {
        Title = request.Title,
        Description = request.Description,
        Tags = request.Tags
      };

      _context.Paths.Add(entity);

      await _context.SaveChangesAsync(cancellationToken);

      return entity.Id;
    }
  }
}
