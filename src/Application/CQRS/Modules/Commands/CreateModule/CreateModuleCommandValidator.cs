using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.CQRS.Modules.Commands.CreateModule
{
  /// <summary>
  /// Validation rules for creating module
  /// </summary>
  public class CreateModuleCommandValidator : AbstractValidator<CreateModule>
  {
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// </summary>
    /// <param name="context"></param>
    public CreateModuleCommandValidator(IApplicationDbContext context)
    {
      _context = context;

      RuleFor(v => v.PathId)
          .NotEmpty().WithMessage("Path Id is required.");

      RuleFor(v => v.Key)
          .NotEmpty().WithMessage("Path Id is required.");

            RuleFor(v => v.Title)
        .NotEmpty().WithMessage("Title is required.")
        .MaximumLength(100).WithMessage("Title must not exceed 100 characters.")
        .MustAsync(BeUniqueTitle).WithMessage("The specified module already exists in this path.");

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
    public async Task<bool> BeUniqueTitle(CreateModule model, string title, CancellationToken cancellationToken)
    {
      //Verify that all modules in path have titles different than title
      var pathModules = await _context.Paths
        .Where(p => p.Id == model.PathId)
        .Include(p => p.Modules)
        .SelectMany(m => m.Modules).ToListAsync(cancellationToken);
        
      return pathModules.All(m => m.Title != title);
    }
  }
}
