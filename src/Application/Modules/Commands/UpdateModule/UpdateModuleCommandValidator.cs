using FluentValidation;

namespace DeveloperPath.Application.Modules.Commands.UpdateModule
{
  public class UpdateModuleCommandValidator : AbstractValidator<UpdateModuleCommand>
  {
    public UpdateModuleCommandValidator()
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
