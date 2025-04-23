using FluentValidation;
using JoygameProject.Application.Features.Commands.Login;

namespace JoygameProject.Application.Validators.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommandRequest>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email boş olamaz")
                .EmailAddress().WithMessage("Geçerli bir email gir.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre boş olamaz");

        }
    }
}
