using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.CQRS.Modules.Commands.UpdateModule
{
  /// <summary>
  /// Validation rules for updating module
  /// </summary>
  public class UpdateModuleCommandValidator : AbstractValidator<UpdateModule>
  {
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// </summary>
    /// <param name="context"></param>
    public UpdateModuleCommandValidator(IApplicationDbContext context)
    {
      _context = context;

      RuleFor(v => v.Title)
        .NotEmpty().WithMessage("Title is required.")
        .MaximumLength(100).WithMessage("Title must not exceed 100 characters.")
        .MustAsync(BeUniqueTitle).WithMessage("Module with this title already exists in one of associated paths.");

      RuleFor(v => v.Description)
        .NotEmpty().WithMessage("Description is required.")
        .MaximumLength(3000).WithMessage("Description must not exceed 3000 characters.");
    }

    /// <summary>
    /// Request to context to check for unique title
    /// </summary>
    /// <param name="model"></param>
    /// <param name="title"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> BeUniqueTitle(UpdateModule model, string title, CancellationToken cancellationToken)
    {
      var pathIds = await _context.Modules
        .Where(m => m.Id == model.Id)
        .SelectMany(m => m.Paths)
        .Select(p => p.Id)
        .ToListAsync(cancellationToken);

      //Verify that all modules in paths where this module exists have titles different than the title
      var pathModules = await _context.Paths
        .Where(p => pathIds.Contains(p.Id))
        .Include(p => p.Modules)
        .SelectMany(m => m.Modules)
        .Select(m => m.Title)
        .ToListAsync(cancellationToken);

      return pathModules.All(m => m != title);
    }
  }
}
