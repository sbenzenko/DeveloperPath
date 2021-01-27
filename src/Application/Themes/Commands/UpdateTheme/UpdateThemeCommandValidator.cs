using FluentValidation;

namespace DeveloperPath.Application.Themes.Commands.UpdateTheme
{
  public class UpdateThemeCommandValidator : AbstractValidator<UpdateThemeCommand>
  {
    public UpdateThemeCommandValidator()
    {
      RuleFor(v => v.Title)
          .MaximumLength(200)
          .NotEmpty();
      RuleFor(v => v.Description)
          .MaximumLength(3000)
          .NotEmpty();
    }
  }
}
