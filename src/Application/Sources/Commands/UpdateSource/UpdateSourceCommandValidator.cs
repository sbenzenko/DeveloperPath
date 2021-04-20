using FluentValidation;

namespace DeveloperPath.Application.Sources.Commands.UpdateSource
{
  /// <summary>
  /// Validation rules for updating source
  /// </summary>
  public class UpdateSourceCommandValidator : AbstractValidator<UpdateSourceCommand>
  {
    /// <summary>
    /// </summary>
    public UpdateSourceCommandValidator()
    {
      RuleFor(v => v.Id)
          .NotEmpty().WithMessage("Source Id is required.");

      RuleFor(v => v.ModuleId)
          .NotEmpty().WithMessage("Module Id is required.");

      RuleFor(v => v.ThemeId)
          .NotEmpty().WithMessage("Theme Id is required.");

      RuleFor(v => v.Title)
        .NotEmpty().WithMessage("Title is required.")
        .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

      RuleFor(v => v.Url)
        .NotEmpty().WithMessage("URL is required.")
        .Matches(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,4}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)")
          .WithMessage("URL must be in valid format, e.g. http://www.domain.com.")
        .MaximumLength(500).WithMessage("URL must not exceed 500 characters.");

      RuleFor(v => v.Description)
        .MaximumLength(10000).WithMessage("Description must not exceed 10000 characters.");
    }
  }
}