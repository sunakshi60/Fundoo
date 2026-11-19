using BussinessLayer.Label.Command;
using FluentValidation;

namespace BussinessLayer.Label.Validator
{
    public class CreateLabelCommandValidator : AbstractValidator<CreateLabelCommand>
    {
        public CreateLabelCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Label name is required.")
                .MaximumLength(100)
                .WithMessage("Label name cannot exceed 100 characters.");

            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("A valid user is required.");
        }
    }
}
