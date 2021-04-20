using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.Themes.Commands.CreateTheme
{
  /// <summary>
  /// Validation rules for creating theme
  /// </summary>
  public class CreateThemeCommandValidator : AbstractValidator<CreateThemeCommand>
  {
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// </summary>
    /// <param name="context"></param>
    public CreateThemeCommandValidator(IApplicationDbContext context)
    {
      _context = context;

      RuleFor(v => v.ModuleId)
          .NotEmpty().WithMessage("Module Id is required.");

      RuleFor(v => v.Title)
        .NotEmpty().WithMessage("Title is required.")
        .MaximumLength(200).WithMessage("Title must not exceed 200 characters.")
        .MustAsync(BeUniqueTitle).WithMessage("The specified theme already exists in this module.");

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
    public async Task<bool> BeUniqueTitle(CreateThemeCommand model, string title, CancellationToken cancellationToken)
    {
      //Verify that all themes in module have titles different than title
      var moduleThemes = await _context.Modules
        .Where(m => m.Id == model.ModuleId)
        .Include(t => t.Themes)
        .SelectMany(t => t.Themes).ToListAsync(cancellationToken);
        
      return moduleThemes.All(t => t.Title != title);
    }
  }
}
