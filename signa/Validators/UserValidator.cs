using System.Data;
using System.Text.RegularExpressions;
using FluentValidation;
using signa.Dto.user;
using signa.Entities;
using signa.Extensions;

namespace signa.Validators;

public class UserValidator : AbstractValidator<CreateUserDto>
{
    public UserValidator()
    {
        RuleFor(user => user.FirstName)
            .NotEmpty().WithMessage("Имя не может быть пустым.")
            .NotNull().WithMessage("Имя не может быть null.")
            .MaximumLength(15).WithMessage("Превышен максимальный размер имени.")
            .Matches("^[А-ЯЁ][а-яё]+$").WithMessage("Неверный формат имени.");

        RuleFor(user => user.LastName)
            .NotEmpty().WithMessage("Фамилия не может быть пустым.")
            .NotNull().WithMessage("Фамилия не может быть null.")
            .MaximumLength(15).WithMessage("Превышен максимальный размер фамилии.")
            .Matches("^[А-ЯЁ][а-яё]+$").WithMessage("Неверный формат фамилии.");

        RuleFor(user => user.Patronymic)
            .NotEmpty().WithMessage("Отчество не может быть пустым.")
            .NotNull().WithMessage("Отчество не может быть null.")
            .MaximumLength(15).WithMessage("Превышен максимальный размер отчества.")
            .Matches("^[А-ЯЁ][а-яё]+$").WithMessage("Неверный формат отчества.");

        RuleFor(user => user.GroupNumber)
            .NotEmpty().WithMessage("Группа не может быть пустой.")
            .NotNull().WithMessage("Группа не может быть null.")
            .Matches(@"^(РИ|НМТ|ФО|МЕН|СТ|УГИ|ЭУ|ФК|ФТ|Х|ПШ|ЭН|РИМ|НМТМ|ФОМ|МЕНМ|СТМ|УГИМ|ЭУМ|ФКМ|ФТМ|ХМ|ПШМ|ЭНМ)-\d{6}$")
            .WithMessage("Неверный формат группы.");
            
        RuleFor(user => user.Email)
            .NotNull()
            .EmailAddress()
            .WithMessage("Некорректный email адрес.");

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Пароль обязателен.")
            .MinimumLength(8).WithMessage("Пароль должен быть не менее 8 символов.")
            .Matches(@"^[A-Za-z\d!@#$%^&*()_+\-=\[\]{};':\""|,.<>\/?`~]+$").WithMessage("Недопустимые символы.")
            .Matches(@"[A-Z]").WithMessage("Пароль должен содержать хотя бы одну заглавную букву.")
            .Matches(@"[a-z]").WithMessage("Пароль должен содержать хотя бы одну строчную букву.")
            .Matches(@"\d").WithMessage("Пароль должен содержать хотя бы одну цифру.");
        RuleFor(user => user.VkLink)
            .Matches(@"^(https?:\/\/)?(www\.)?vk\.com\/[a-zA-Z0-9_.]+$")
            .When(x => x.VkLink != null)
            .WithMessage("Неверно указана ссылка на вк.");
        
    }
}