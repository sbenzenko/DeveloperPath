using FluentValidation;
using DeveloperPath.Application.Common.Interfaces.Validation;

namespace DeveloperPath.Application.CQRS.Paths.Commands.CreatePath;

/// <summary>
/// Validation rules for creating path
/// </summary>
public class CreatePathCommandValidator : AbstractValidator<CreatePath>
{
  private readonly IUniqueNamingValidation _namingValidation;

  /// <summary>
  /// </summary>
  /// <param name="namingValidation"></param>
  public CreatePathCommandValidator(IUniqueNamingValidation namingValidation)
  {
    _namingValidation = namingValidation;

    RuleFor(v => v.Title)
        .NotEmpty().WithMessage("Title is required.")
        .MaximumLength(100).WithMessage("Title must not exceed 100 characters.")
        .MustAsync((path, title, cancellationToken) => _namingValidation.BeUniqueTitle(default, title, cancellationToken))
        .WithMessage("The specified path already exists.");

    RuleFor(v => v.Key)
        .NotEmpty().WithMessage("URI key is required.")
        .MaximumLength(100).WithMessage("PathKey must not exceed 100 characters.")
        .MustAsync((path, key, cancellationToken) => _namingValidation.BeUniqueKey(default, key, cancellationToken))
        .WithMessage("The specified path key already exists.");

    RuleFor(v => v.Description)
        .NotEmpty().WithMessage("Description is required.")
        .MaximumLength(3000)
        .WithMessage("Description must not exceed 3000 characters.");
  }
}
