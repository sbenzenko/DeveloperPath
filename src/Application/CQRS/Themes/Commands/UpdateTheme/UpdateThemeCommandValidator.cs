using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.CQRS.Themes.Commands.UpdateTheme
{
  /// <summary>
  /// Validation rules for updating theme
  /// </summary>
  public class UpdateThemeCommandValidator : AbstractValidator<UpdateTheme>
  {
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// </summary>
    /// <param name="context"></param>
    public UpdateThemeCommandValidator(IApplicationDbContext context)
    {
      _context = context;

      RuleFor(v => v.Id)
          .NotEmpty().WithMessage("Theme Id is required.");

      RuleFor(v => v.ModuleId)
          .NotEmpty().WithMessage("Module Id is required.");

      RuleFor(v => v.Title)
        .NotEmpty().WithMessage("Title is required.")
        .MaximumLength(200).WithMessage("Title must not exceed 200 characters.")
        .MustAsync(BeUniqueTitle).WithMessage("Theme with this title already exists in this module.");

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
    public async Task<bool> BeUniqueTitle(UpdateTheme model, string title, CancellationToken cancellationToken)
    {
      //Verify that all themes in module have titles different than title
      var moduleThemes = await _context.Modules
        .Where(m => m.Id == model.ModuleId)
        .Include(t => t.Themes)
        .SelectMany(t => t.Themes)
        .Select(t => t.Title)
        .ToListAsync(cancellationToken);

      return moduleThemes.All(t => t != title);
    }
  }
}
