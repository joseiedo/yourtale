using FluentValidation;
using YourTale.Application.Contracts.Documents.Requests.User;

namespace YourTale.Application.Validators.User;

public class UserEditValidator : AbstractValidator<UserEditRequest>
{
    public UserEditValidator()
    {
        RuleFor(x => x.Nickname).MaximumLength(50).WithMessage("O apelido deve ter até 50 caracteres");
        RuleFor(x => x.Picture).MaximumLength(512).WithMessage("A URL da foto deve ter até 512 caracteres");
    }
}