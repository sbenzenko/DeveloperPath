using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Application.Modules.Commands.DeleteModule
{
  public record DeleteModuleCommand : IRequest
  {
    public int Id { get; init; }
  }

  public class DeletePathCommandHandler : IRequestHandler<DeleteModuleCommand>
  {
    private readonly IApplicationDbContext _context;

    public DeletePathCommandHandler(IApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<Unit> Handle(DeleteModuleCommand request, CancellationToken cancellationToken)
    {
      var entity = await _context.Modules.FindAsync(new object[] { request.Id }, cancellationToken);

      if (entity == null)
      {
        throw new NotFoundException(nameof(Module), request.Id);
      }

      _context.Modules.Remove(entity);

      await _context.SaveChangesAsync(cancellationToken);

      return Unit.Value;
    }
  }
}
