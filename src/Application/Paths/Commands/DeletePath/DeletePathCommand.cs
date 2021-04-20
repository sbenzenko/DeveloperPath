using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Application.Paths.Commands.DeletePath
{
  /// <summary>
  /// Delete path parameters
  /// </summary>
  public record DeletePathCommand : IRequest
  {
    /// <summary>
    /// Path ID
    /// </summary>
    public int Id { get; init; }
  }

  internal class DeletePathCommandHandler : IRequestHandler<DeletePathCommand>
  {
    private readonly IApplicationDbContext _context;

    public DeletePathCommandHandler(IApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<Unit> Handle(DeletePathCommand request, CancellationToken cancellationToken)
    {
      var entity = await _context.Paths.FindAsync(new object[] { request.Id }, cancellationToken);
      if (entity == null)
        throw new NotFoundException(nameof(Path), request.Id);

      _context.Paths.Remove(entity);

      await _context.SaveChangesAsync(cancellationToken);

      return Unit.Value;
    }
  }
}
