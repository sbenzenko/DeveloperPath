using System.Threading;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.Paths.Commands.CreatePath
{
  /// <summary>
  /// Validation rules for creating path
  /// </summary>
  public class CreatePathCommandValidator : AbstractValidator<CreatePath>
  {
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// </summary>
    /// <param name="context"></param>
    public CreatePathCommandValidator(IApplicationDbContext context)
    {
      _context = context;

      RuleFor(v => v.Title)
        .NotEmpty().WithMessage("Title is required.")
        .MaximumLength(100).WithMessage("Title must not exceed 100 characters.")
        .MustAsync(BeUniqueTitle).WithMessage("The specified path already exists.");

      RuleFor(v => v.Description)
        .NotEmpty().WithMessage("Description is required.")
        .MaximumLength(3000).WithMessage("Description must not exceed 3000 characters.");
    }

    /// <summary>
    /// Request to context to check for unique title
    /// </summary>
    /// <param name="title"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
    {
      return await _context.Paths
        .AllAsync(l => l.Title != title, cancellationToken);
    }
  }
}
