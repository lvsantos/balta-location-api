namespace Baltaio.Location.Api.Infrastructure.Users.Authentication;

internal class JwtSettings
{
    public const string SECTION_NAME = "JwtSettings";

    public string Secret { get; init; } = string.Empty;
    public int ExpirationInMinutes { get; init; }
    public string Issuer { get; init; } = string.Empty;

}
