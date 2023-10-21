using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Application.Addresses.CreateAddress;
using Baltaio.Location.Api.Application.Addresses.GetAddress;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Baltaio.Location.Api.Controllers.Addresses;

[ApiController]
[Route("api/[controller]")]
public class AddressesController : ControllerBase
{
    private readonly ICityRepository _cityRepository;
    private readonly IAddressRepository _addressRepository;


    public AddressesController(ICityRepository cityRepository, IAddressRepository addressRepository)
    {
        _cityRepository = cityRepository;
        _addressRepository = addressRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAddressAsync(CreateAddressRequest request)
    {
        CreateCityAppService service = new(_cityRepository, _addressRepository);
        CreateCityInput input = new(request.IbgeCode, request.NameCity, request.StateCode);

        CreateCityOutput output = await service.ExecuteAsync(input);

        if (output.Valid == false)
        {
            ModelStateDictionary keyValuePairs = new();
            keyValuePairs.AddModelError(string.Empty, output.Message);
            return ValidationProblem(keyValuePairs);
        }

        return CreatedAtAction(nameof(Get), new { id = request.IbgeCode}, null);
    }

    //<summary>
    /// <summary>
    /// Lista todos os dados de um endereço
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        GetCityAppService service = new(_cityRepository, _addressRepository);      

        GetCityOutput output = await service.ExecuteAsync(id);
        
        if (output.Valid == false)
        {
            ModelStateDictionary keyValuePairs = new();
            keyValuePairs.AddModelError(string.Empty, output.Message);
            return ValidationProblem(keyValuePairs);
        }
        GetCityResponse GetCityResponse = new (output.IbgeCode, output.NameCity, output.StateCode)
        {
             IbgeCode = output.IbgeCode,
             NameCity = output.NameCity,
             StateCode = output.StateCode   
        };
        return Ok(GetCityResponse);
    }
}
