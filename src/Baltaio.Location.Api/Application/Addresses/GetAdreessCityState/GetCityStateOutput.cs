using Baltaio.Location.Api.Domain;

namespace Baltaio.Location.Api.Application.Addresses.GetAdreessCityState;

public record GetCityStateOutput(
    int? IbgeCode,
    string CityName,
    int? StateCode,
    string StateAbreviation,
    string StateName,
    bool IsValid,
    string ErrorMessage)
{
    public static GetCityStateOutput Validation() =>
        new(
            IbgeCode: null,
            CityName: string.Empty,
            StateCode: null,
            StateAbreviation: string.Empty,
            StateName: string.Empty,
            IsValid: false,
            ErrorMessage: "Cidade não encontrada.");
    public static GetCityStateOutput Success(City city) =>
        new(
            IbgeCode: city.Code,
            CityName: city.Name,
            StateCode: city.State.Code,
            StateAbreviation: city.State.Abbreviation,
            StateName: city.State.Name,
            IsValid: true,
            ErrorMessage: string.Empty);

    public static List<GetCityStateOutput> Success(List<City> cities)
    {
        List<GetCityStateOutput> outputs = cities.Select(c =>
        new GetCityStateOutput(
            IbgeCode: c.Code,
            CityName: c.Name,
            StateCode: c.State.Code,
            StateAbreviation: c.State.Abbreviation,
            StateName: c.State.Name,
            IsValid: true,
            ErrorMessage: string.Empty
            )
        ).ToList();
        return outputs;
    }
}