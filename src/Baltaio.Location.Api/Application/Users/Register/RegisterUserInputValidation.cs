using Flunt.Validations;

namespace Baltaio.Location.Api.Application.Users.Register;

public class RegisterUserInputValidation : Contract<RegisterUserInput>
{
    public RegisterUserInputValidation(RegisterUserInput input)
    {
        Requires()
            .IsEmail(input.Email, "Email.Invalid", "O email informado é inválido")
            .IsGreaterThan(input.Password, 7, "Password.Invalid", "A senha deve conter no mínimo 8 caracteres")
            .IsNotNull(input.Password, "Password.Invalid", "A senha deve conter no mínimo 8 caracteres");
    }
}


