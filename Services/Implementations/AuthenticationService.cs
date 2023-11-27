using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MysteryGuestAPI.Services.Interfaces;

namespace MysteryGuestAPI.Services.Implementations;

public class AuthenticationService : IAuthenticationService
{
    private readonly IConfiguration _configuration;

    public AuthenticationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value!));
        var signingCredentials = new SigningCredentials(securityKey, algorithm: SecurityAlgorithms.HmacSha256);

        _ = int.TryParse(_configuration["Jwt:TokenValidityInMinutes"], out int tokenValidityInMinutes);
        
        var securityToken = new JwtSecurityToken(
            claims: claims, 
            expires: DateTime.Now.AddMinutes(tokenValidityInMinutes), 
            issuer: _configuration.GetSection("Jwt:Issuer").Value, 
            audience: _configuration.GetSection("Jwt:Issuer").Value, 
            signingCredentials: signingCredentials);

        return securityToken;
    }
    
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}