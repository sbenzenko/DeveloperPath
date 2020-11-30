using FluentValidation;

namespace DeveloperPath.Application.Paths.Commands.CreatePath
{
  public class CreatePathCommandValidator : AbstractValidator<CreatePathCommand>
  {
    public CreatePathCommandValidator()
    {
      RuleFor(v => v.Title)
          .MaximumLength(100)
          .NotEmpty();
      RuleFor(v => v.Description)
          .MaximumLength(3000)
          .NotEmpty();
    }
  }
}
