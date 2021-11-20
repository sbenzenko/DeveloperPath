using FluentValidation;

namespace DeveloperPath.Application.CQRS.Paths.Commands.CreatePath
{
    /// <summary>
    /// Validation rules for creating path
    /// </summary>
    public class CreatePathCommandValidator : AbstractValidator<CreatePath>
    {
        private readonly IBasePathValidation _pathValidation;

        /// <summary>
        /// </summary>
        /// <param name="pathValidation"></param>
        public CreatePathCommandValidator(IBasePathValidation pathValidation)
        {
            _pathValidation = pathValidation;

            RuleFor(v => v.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.")
                .MustAsync((path, title, cancellationToken) => _pathValidation.BeUniqueTitle(default, title, cancellationToken))
                .WithMessage("The specified path already exists.");

            RuleFor(v => v.Key)
                .NotEmpty().WithMessage("URI key is required.")
                .MaximumLength(100).WithMessage("PathKey must not exceed 100 characters.")
                .MustAsync((path, key, cancellationToken) => _pathValidation.BeUniqueKey(default, key, cancellationToken))
                .WithMessage("The specified path key already exists.");

            RuleFor(v => v.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(3000)
                .WithMessage("Description must not exceed 3000 characters.");
        }
    }
}
