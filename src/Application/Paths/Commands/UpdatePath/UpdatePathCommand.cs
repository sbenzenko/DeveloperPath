using AutoMapper;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Application.Common.Models;
using DeveloperPath.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Application.Paths.Commands.UpdatePath
{
  /// <summary>
  /// Path to update
  /// </summary>
  public record UpdatePathCommand : IRequest<PathDto>
  {
    /// <summary>
    /// Id of the path to update
    /// </summary>
    [Required]
    public int Id { get; init; }
    /// <summary>
    /// Path title
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Title { get; init; }
    /// <summary>
    /// Path short summary
    /// </summary>
    [Required]
    [MaxLength(3000)]
    public string Description { get; init; }
    /// <summary>
    /// List of tags related to path
    /// </summary>
    public IList<string> Tags { get; init; }
  }

  internal class UpdatePathCommandHandler : IRequestHandler<UpdatePathCommand, PathDto>
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
