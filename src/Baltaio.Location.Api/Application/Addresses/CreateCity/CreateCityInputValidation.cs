using Flunt.Validations;

namespace Baltaio.Location.Api.Application.Addresses.CreateCity;

public class CreateCityInputValidation : Contract<CreateCityInput>
{
    public CreateCityInputValidation(CreateCityInput input)
    {
        Requires()
            .IsGreaterThan(input.IbgeCode, 0, "IbgeCode.Invalid", "Código do IBGE inválido.")
            .IsNotNullOrEmpty(input.Name, "CityName.Invalid", "Nome da cidade inválido.")
            .IsGreaterThan(input.StateCode, 0, "StateCode.Invalid", "Código do estado inválido.");
    }
}