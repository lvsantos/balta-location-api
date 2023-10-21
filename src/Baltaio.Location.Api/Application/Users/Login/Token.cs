namespace Baltaio.Location.Api.Application.Users.Login;

public class Token
{
    public Token(string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value, nameof(value));

        Value = value;
    }

    public string Value { get; init; }
}
