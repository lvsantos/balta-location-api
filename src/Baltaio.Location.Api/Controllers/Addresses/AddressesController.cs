using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Application.Addresses.CreateAddress;
using Microsoft.AspNetCore.Mvc;

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
        CreateAddressAppService service = new(_cityRepository, _addressRepository);
        CreateAddressInput input = new(request. IbgeCode);

        CreateAddressOutput output = await service.ExecuteAsync(input);

        if (output.AddressCode == Guid.Empty)
            return BadRequest(output.Message);

        return CreatedAtAction(nameof(Get), new { id = output.AddressCode }, output);
    }

    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        return Ok();
    }
}
