using FluentValidation;
using signa.Entities;

namespace signa.Validators;

public class UserValidator : AbstractValidator<UserEntity>
{
    public UserValidator()
    {
        RuleFor(user => user.FirstName)
            .NotEmpty().WithMessage("FirstName cannot be empty");
        RuleFor(user => user.LastName)
            .NotEmpty().WithMessage("LastName cannot be empty");
    }
}