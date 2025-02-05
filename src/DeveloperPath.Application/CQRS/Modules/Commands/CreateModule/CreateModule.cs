using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Shared.ClientModels;
using DeveloperPath.Shared.Enums;

using MediatR;

namespace DeveloperPath.Application.CQRS.Modules.Commands.CreateModule;

/// <summary>
/// A module to create
/// </summary>
public record CreateModule : IRequest<Module>
{
  /// <summary>
  /// Module title
  /// </summary>
  [Required]
  [MaxLength(100)]
  public string Title { get; init; }

  /// <summary>
  /// Module unique URI key
  /// </summary>
  [Required]
  [MaxLength(100)]
  public string Key { get; init; }

  /// <summary>
  /// Module short summary
  /// </summary>
  [Required]
  [MaxLength(3000)]
  public string Description { get; init; }
  /// <summary>
  /// Necessity level (Other (default) | Possibilities | Interesting | Good to know | Must know)
  /// </summary>
  public Necessity Necessity { get; init; }
  /// <summary>
  /// Position of module in path (0-based)
  /// </summary>
  public int Order { get; init; }
  /// <summary>
  /// List of tags related to the module
  /// </summary>
  public IList<string> Tags { get; init; }
}

internal class CreateModuleCommandHandler : IRequestHandler<CreateModule, Module>
{
  private readonly IApplicationDbContext _context;
  private readonly IMapper _mapper;

  public CreateModuleCommandHandler(IApplicationDbContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }

  public async Task<Module> Handle(CreateModule request, CancellationToken cancellationToken)
  {
    var entity = new Domain.Entities.Module
    {
      Title = request.Title,
      Key = request.Key,
      Description = request.Description,
      Necessity = request.Necessity,
      Tags = request.Tags,
    };

    _context.Modules.Add(entity);

    await _context.SaveChangesAsync(cancellationToken);

    return _mapper.Map<Module>(entity);
  }
}
