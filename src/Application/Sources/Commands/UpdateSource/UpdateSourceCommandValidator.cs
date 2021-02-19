using FluentValidation;

namespace DeveloperPath.Application.Sources.Commands.UpdateSource
{
  public class UpdateSourceCommandValidator : AbstractValidator<UpdateSourceCommand>
  {
    public UpdateSourceCommandValidator()
    {
      RuleFor(v => v.Id)
          .NotEmpty().WithMessage("Source Id is required.");

      RuleFor(v => v.ThemeId)
          .NotEmpty().WithMessage("Theme Id is required.");

      RuleFor(v => v.ModuleId)
          .NotEmpty().WithMessage("Module Id is required.");

      RuleFor(v => v.Title)
        .NotEmpty().WithMessage("Title is required.")
        .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

      RuleFor(v => v.Url)
        .NotEmpty().WithMessage("URL is required.")
        .MaximumLength(500).WithMessage("URL must not exceed 500 characters.");

      RuleFor(v => v.Description)
        .MaximumLength(10000).WithMessage("Description must not exceed 10000 characters.");
    }
  }
}