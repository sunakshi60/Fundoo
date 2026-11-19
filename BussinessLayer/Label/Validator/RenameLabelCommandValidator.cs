using FluentValidation;
using BussinessLayer.Label.Command;

namespace BussinessLayer.Label.Validator
{
    public class RenameLabelCommandValidator : AbstractValidator<RenameLabelCommand>
    {
        public RenameLabelCommandValidator()
        {
            RuleFor(x => x.LabelId)
                .GreaterThan(0)
                .WithMessage("A valid label ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Label name is required.")
                .Must(name => !string.IsNullOrWhiteSpace(name))
                .WithMessage("Label name cannot contain only whitespace.")
                .MaximumLength(100)
                .WithMessage("Label name cannot exceed 100 characters.");

            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("A valid user is required.");
        }
    }
}
