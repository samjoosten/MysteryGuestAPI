using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MysteryGuestAPI.Contexts;
using MysteryGuestAPI.DbContext;
using MysteryGuestAPI.Dtos;

namespace MysteryGuestAPI.Handlers.User;

public static class GetUserHandler
{
    public static async Task<IResult> GetUser(ClaimsPrincipal claims, ApplicationDbContext context, [FromServices] UserManager<ApplicationUser> userManager)
    {
        var email = claims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        if (email is null)
        {
            return TypedResults.BadRequest();
        }
        
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return TypedResults.BadRequest();
        }
        
        return TypedResults.Ok(user.ToDto());
    }
}