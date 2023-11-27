using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MysteryGuestAPI.Contexts;
using MysteryGuestAPI.DbContext;
using MysteryGuestAPI.Handlers.Authentication.Shared;
using MysteryGuestAPI.Services.Interfaces;

namespace MysteryGuestAPI.Handlers.Authentication;

public record TokenGenerationRequest(string? AccessToken, string? RefreshToken);

public static class RefreshAccessTokenHandler
{
    public static async Task<IResult> RefreshAccessToken([FromBody] TokenGenerationRequest request, 
        [FromServices] IAuthenticationService authenticationService, 
        [FromServices] UserManager<ApplicationUser> userManager, 
        [FromServices] IConfiguration configuration, ApplicationDbContext context)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var issuerData = tokenHandler.ValidateToken(request.AccessToken, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityException("Invalid token");

        var emailClaim = issuerData.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

        if (emailClaim is null)
        {
            throw new SecurityException("Invalid claims");
        }
        
        var user = await userManager.FindByEmailAsync(emailClaim.Value);
        
        if (user.RefreshToken != request.RefreshToken)
        {
            return TypedResults.Unauthorized();
        }

        var newAccessToken = authenticationService.GenerateAccessToken(issuerData.Claims);
        var newRefreshToken = authenticationService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(int.Parse(configuration["Jwt:RefreshTokenValidityInDays"]!));
        
        await userManager.UpdateAsync(user);
        await context.SaveChangesAsync();

        return TypedResults.Ok(new AccessTokenResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            RefreshToken = newRefreshToken,
            Expiration = newAccessToken.ValidTo
        });
    }
}