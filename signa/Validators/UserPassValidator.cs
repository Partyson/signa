using FluentValidation;
using signa.Dto.user;

namespace signa.Validators;

public class UserPassValidator : AbstractValidator<UpdateUserPassDto>
{
    public UserPassValidator()
    {
        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Пароль обязателен.")
            .MinimumLength(8).WithMessage("Пароль должен быть не менее 8 символов.")
            .Matches(@"^[A-Za-z\d!@#$%^&*()_+\-=\[\]{};':\""|,.<>\/?`~]+$").WithMessage("Недопустимые символы.")
            .Matches(@"[A-Z]").WithMessage("Пароль должен содержать хотя бы одну заглавную букву.")
            .Matches(@"[a-z]").WithMessage("Пароль должен содержать хотя бы одну строчную букву.")
            .Matches(@"\d").WithMessage("Пароль должен содержать хотя бы одну цифру.");
    }
}