using Flunt.Validations;

namespace Baltaio.Location.Api.Application.Users.Login;

public class LoginInputValidation : Contract<LoginInput>
{
    public LoginInputValidation(LoginInput input)
    {
        Requires()
            .IsEmail(input.Email, "Email.Invalid", "O email informado é inválido.")
            .IsNotNullOrEmpty(input.Password, "Password.Invalid", "A senha não pode ser vazia.");
    }
}
