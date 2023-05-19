using FluentValidation;
using YourTale.Application.Contracts.Documents.Requests.User;

namespace YourTale.Application.Validators.User;

public class UserRegisterValidator : AbstractValidator<UserRegisterRequest>
{
    public UserRegisterValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().WithMessage("O nome completo é obrigatório").MaximumLength(255);
        RuleFor(x => x.Email).EmailAddress().WithMessage("O email informado não é válido");
        RuleFor(x => x.Nickname).MaximumLength(50).WithMessage("O apelido deve ter até 50 caracteres");
        RuleFor(x => x.BirthDate).NotEmpty().WithMessage("A data de nascimento é obrigatória");
        RuleFor(x => x.Cep).Must(ValidateCep).WithMessage("O CEP deve ter 8 caracteres");
        RuleFor(x => x.Password).MinimumLength(3).MaximumLength(128)
            .WithMessage("A senha deve ter entre 3 e 128 caracteres");
        RuleFor(x => x.ConfirmPassword)
            .Must((model, field) => field == model.Password).WithMessage("As senhas não coincidem");
        RuleFor(x => x.Picture).MaximumLength(512).WithMessage("A URL da foto deve ter até 512 caracteres");
    }

    private static bool ValidateCep(string cep)
    {
        return cep.Replace("-", "").Length == 8;
    }
}