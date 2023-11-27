using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MysteryGuestAPI.Contexts;

namespace MysteryGuestAPI.Handlers.Authentication;

public record CustomRegisterRequest(string Email, string Password, string FirstName, string LastName, Role Role);

public static class RegisterHandler
{
    public static async Task<IResult> Register([FromBody] CustomRegisterRequest request, 
        [FromServices] UserManager<ApplicationUser> userManager, [FromServices] RoleManager<IdentityRole> roleManager)
    {
        var applicationUser = new ApplicationUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = request.Role,
            UserName = $"{request.FirstName} {request.LastName}",
            Email = request.Email
        };

        var result = await userManager.CreateAsync(applicationUser, request.Password);

        if (!result.Succeeded)
        {
            return TypedResults.BadRequest();
        }

        if (!await roleManager.RoleExistsAsync(request.Role.ToString()))
        {
            await roleManager.CreateAsync(new IdentityRole(request.Role.ToString()));
        }
        
        await userManager.AddToRoleAsync(applicationUser, request.Role.ToString());

        return TypedResults.Ok();
    }
}