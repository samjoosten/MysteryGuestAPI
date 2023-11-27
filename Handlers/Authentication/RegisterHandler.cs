using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MysteryGuestAPI.Attributes;
using MysteryGuestAPI.Contexts;
using MysteryGuestAPI.DbContext;

namespace MysteryGuestAPI.Handlers.Authentication;

public record CustomRegisterRequest(string Password, string FirstName, string LastName, string InviteToken);

public static class RegisterHandler
{
    public static async Task<IResult> Register([FromBody] CustomRegisterRequest request, ClaimsPrincipal claims, ApplicationDbContext context,
        [FromServices] UserManager<ApplicationUser> userManager, [FromServices] RoleManager<IdentityRole> roleManager)
    {
        var invite = await context.UserInvites.FirstOrDefaultAsync(x => x.Token == request.InviteToken);
        if (invite is null || !invite.IsValid())
        {
            return TypedResults.BadRequest("Failed to register, invalid token");
        }
        
        var applicationUser = new ApplicationUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = invite.Role,
            UserName = invite.Email,
            Email = invite.Email
        };

        var result = await userManager.CreateAsync(applicationUser, request.Password);

        if (!result.Succeeded)
        {
            return TypedResults.BadRequest();
        }

        if (!await roleManager.RoleExistsAsync(invite.Role.ToString()))
        {
            await roleManager.CreateAsync(new IdentityRole(invite.Role.ToString()));
        }
        
        await userManager.AddToRoleAsync(applicationUser, invite.Role.ToString());
        
        context.UserInvites.Remove(invite);
        await context.SaveChangesAsync();
        
        return TypedResults.Ok();
    }
}