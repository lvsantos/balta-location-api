using Baltaio.Location.Api.Application.Users.Login;
using Baltaio.Location.Api.Application.Users.Login.Abstractions;
using Baltaio.Location.Api.Domain.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Baltaio.Location.Api.Infrastructure.Authentication;

internal class JwtGenerator : IJwtGenerator
{
    private readonly JwtSettings _jwtSettings;

    public JwtGenerator(IOptions<JwtSettings> jwtSettingsOptions)
    {
        _jwtSettings = jwtSettingsOptions.Value;
    }

    public Token GenerateToken(User user)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        Claim[] claims = GetClaims(user);
        SigningCredentials signingCredentials = CreateSignature();

        Token token = CreateToken(claims, signingCredentials);

        return token;
    }

    private static Claim[] GetClaims(User user)
    {
        return new[]
        {
            new Claim(ClaimTypes.Name, user.Code.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };
    }
    private SigningCredentials CreateSignature()
    {
        byte[] key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);

        SigningCredentials credentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

        return credentials;
    }
    private Token CreateToken(Claim[] claims, SigningCredentials signingCredentials)
    {
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationInMinutes),
            SigningCredentials = signingCredentials
        };
        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return new(tokenHandler.WriteToken(token));
    }
}
