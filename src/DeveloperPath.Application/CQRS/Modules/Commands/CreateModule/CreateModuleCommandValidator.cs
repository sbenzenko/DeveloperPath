using FluentValidation;

namespace DeveloperPath.Application.CQRS.Modules.Commands.CreateModule
{
    /// <summary>
    /// Validation rules for creating module
    /// </summary>
    public class CreateModuleCommandValidator : AbstractValidator<CreateModule>
    {
        /// <summary>
        /// CreateModuleCommand validator
        /// </summary>
        public CreateModuleCommandValidator()
        { 
            RuleFor(v => v.Key)
                .NotEmpty().WithMessage("Path Id is required."); 

            RuleFor(v => v.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100)
                .WithMessage("Title must not exceed 100 characters.");

            RuleFor(v => v.Description)
              .NotEmpty().WithMessage("Description is required.")
              .MaximumLength(3000).WithMessage("Description must not exceed 3000 characters.");
        }
    }
}
