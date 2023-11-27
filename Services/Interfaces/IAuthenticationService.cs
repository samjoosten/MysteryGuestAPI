using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MysteryGuestAPI.Services.Interfaces;

public interface IAuthenticationService
{
    public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims);
    public string GenerateRefreshToken();
}