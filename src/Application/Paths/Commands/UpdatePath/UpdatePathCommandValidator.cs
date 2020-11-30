using FluentValidation;

namespace DeveloperPath.Application.Paths.Commands.UpdatePath
{
  public class UpdatePathCommandValidator : AbstractValidator<UpdatePathCommand>
  {
    public UpdatePathCommandValidator()
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
