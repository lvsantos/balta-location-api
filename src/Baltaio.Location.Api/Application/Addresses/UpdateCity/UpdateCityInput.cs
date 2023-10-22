using Flunt.Notifications;

namespace Baltaio.Location.Api.Application.Addresses.UpdateCity;

public sealed class UpdateCityInput : Notifiable<Notification>
{
    public UpdateCityInput(int ibgeCode, string name, int stateCode)
    {
        IbgeCode = ibgeCode;
        Name = name;
        StateCode = stateCode;

        AddNotifications(new UpdateCityInputValidation(this));
    }

    public int IbgeCode { get; init; }
    public string Name { get; init; }
    public int StateCode { get; init; }
}
