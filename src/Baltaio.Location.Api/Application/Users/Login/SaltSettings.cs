namespace Baltaio.Location.Api.Application.Users.Login;

public class SaltSettings
{
    public const string SECTION_NAME = "SaltSettings";

    public string Salt { get; init; } = string.Empty;
}
