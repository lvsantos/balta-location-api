using Flunt.Validations;

namespace Baltaio.Location.Api.Application.Addresses.UpdateCity;

public class UpdateCityInputValidation : Contract<UpdateCityInput>
{
    public UpdateCityInputValidation(UpdateCityInput input)
    {
        Requires()
            .IsGreaterThan(input.IbgeCode, 0, "IbgeCode.Mandatory", "O código do IBGE é obrigatório.")
            .IsNotNullOrEmpty(input.Name, "Name.Mandatory", "O nome da cidade é obrigatório.")
            .IsGreaterThan(input.StateCode, 0, "StateCode.Mandatory", "O código do estado é obrigatório.");
    }
}
