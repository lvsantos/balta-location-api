namespace Baltaio.Location.Api.Application.Addresses.CreateAddress.Abstractions;

public interface ICreateCityAppService
{
    Task<CreateCityOutput> ExecuteAsync(CreateCityInput input);
}
