using Baltaio.Location.Api.Domain;

namespace Baltaio.Location.Api.Application.Addresses.GetAddress;

public record GetCityOutput(int? IbgeCode,
        string CityName,
        int? StateCode,
        string StateAbreviation,
        string Statename, 
        bool IsValid, 
        string ErrorMessage)
{
    public static GetCityOutput Validation() =>
        new(
            IbgeCode: null,
            CityName: string.Empty,
            StateCode: null,
            StateAbreviation: string.Empty,
            Statename: string.Empty,
            IsValid: false,
            ErrorMessage: "Cidade não encontrada.");
    public static GetCityOutput Success(City city) =>
        new(
            IbgeCode: city.Code,
            CityName: city.Name,
            StateCode: city.State.Code,
            StateAbreviation: city.State.Abbreviation,
            Statename: city.State.Name,
            IsValid: true,
            ErrorMessage: string.Empty);
}