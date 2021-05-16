using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Application.CQRS.Paths.Commands.DeletePath
{
  /// <summary>
  /// Delete path parameters
  /// </summary>
  public record DeletePath : IRequest
  {
    /// <summary>
    /// Path ID
    /// </summary>
    [Required]
    public int Id { get; init; }
  }

  public class DeletePathCommandHandler : IRequestHandler<DeletePath>
  {
    private readonly IApplicationDbContext _context;

    public DeletePathCommandHandler(IApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<Unit> Handle(DeletePath request, CancellationToken cancellationToken)
    {
      var entity = await _context.Paths.FindAsync(new object[] { request.Id }, cancellationToken);
      if (entity == null)
        throw new NotFoundException(nameof(Path), request.Id, NotFoundHelper.PATH_NOT_FOUND);

      _context.Paths.Remove(entity);

      await _context.SaveChangesAsync(cancellationToken);
      return Unit.Value;
    }
  }
}
