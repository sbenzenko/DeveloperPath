using System.Threading;
using FluentValidation;

namespace DeveloperPath.Application.CQRS.Paths.Commands.UpdatePath
{
    /// <summary>
    /// Validation rules for updating path
    /// </summary>
    public class UpdatePathCommandValidator : AbstractValidator<UpdatePath>
    {
        private readonly IBasePathValidation _pathValidation;

        public UpdatePathCommandValidator(IBasePathValidation pathValidation)
        {
            _pathValidation = pathValidation;


            RuleFor(v => v.Title)
              .NotEmpty().WithMessage("Title is required.")
              .MaximumLength(100).WithMessage("Title must not exceed 100 characters.")
              .MustAsync((path, title, cancellationToken) => _pathValidation.BeUniqueTitle(path.Id, title, cancellationToken))
              .WithMessage("The specified path already exists.");

            RuleFor(v => v.Key)
                .NotEmpty().WithMessage("URI key is required.")
                .MaximumLength(100).WithMessage("Key must not exceed 100 characters.")
                .MustAsync((path, key, cancellationToken) => _pathValidation.BeUniqueKey(path.Id, key, cancellationToken))
                .WithMessage("The specified path key already exists.");

            RuleFor(v => v.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(3000)
                .WithMessage("Description must not exceed 3000 characters.");
        }
    }
}
