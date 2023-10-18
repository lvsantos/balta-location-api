using Baltaio.Location.Api.Application.Users.Login;
using Baltaio.Location.Api.Application.Users.Login.Abstractions;
using Baltaio.Location.Api.Domain.Users;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Baltaio.Location.Api.Infrastructure.Users.Authentication;

internal class JwtGenerator : IJwtGenerator
{
    /*private readonly JwtSettings _jwtSettings;

    public JwtGenerator(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }*/

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
            new Claim(ClaimTypes.Name, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };
    }
    private static SigningCredentials CreateSignature()
    {
        //byte[] key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
        byte[] key = Encoding.ASCII.GetBytes("12345678901234567890123456789012");

        SigningCredentials credentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

        return credentials;
    }
    private static Token CreateToken(Claim[] claims, SigningCredentials signingCredentials)
    {
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Issuer = "Baltaio.FirstChallenge",
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = signingCredentials
        };
        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return new(tokenHandler.WriteToken(token));
    }
}
