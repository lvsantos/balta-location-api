using Baltaio.Location.Api.Application.Addresses.GetAddress;

namespace Baltaio.Location.Api.Controllers.Addresses
{
    public record GetCityResponse(int? IbgeCode, string NameCity, int? StateCode);
}
