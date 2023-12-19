using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MysteryGuestAPI.Contexts;
using MysteryGuestAPI.DbContext;
using MysteryGuestAPI.Handlers.Authentication.Shared;
using MysteryGuestAPI.Repositories;
using MysteryGuestAPI.Services.Interfaces;

namespace MysteryGuestAPI.Handlers.Authentication;

public record LoginRequest(string Email, string Password);

public static class LoginHandler
{
    public static async Task<IResult> Login([FromBody] LoginRequest request,
        [FromServices] IAuthenticationService authenticationService,
        [FromServices] UserManager<ApplicationUser> userManager, [FromServices] IConfiguration configuration)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        
        if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
        {
            return TypedResults.Unauthorized();
        }

        var userRoles = await userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.FullName()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var token = authenticationService.GenerateAccessToken(authClaims);
        var refreshToken = authenticationService.GenerateRefreshToken();
        
        _ = int.TryParse(configuration["Jwt:RefreshTokenValidityInDays"], out var refreshTokenValidityInDays);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenValidityInDays);
        
        await userManager.UpdateAsync(user);

        return TypedResults.Ok(new AccessTokenResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken,
            Expiration = token.ValidTo
        });
    }
}

