using Flunt.Notifications;

namespace Baltaio.Location.Api.Application.Addresses.CreateCity;

public class CreateCityInput : Notifiable<Notification>
{
    public CreateCityInput(int ibgeCode, string name, int stateCode)
    {
        IbgeCode = ibgeCode;
        Name = name;
        StateCode = stateCode;

        AddNotifications(new CreateCityInputValidation(this));
    }

    public int IbgeCode { get; init; }
    public string Name { get; init; }
    public int StateCode { get; init; }
}
