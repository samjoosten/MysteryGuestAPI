using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MysteryGuestAPI.Contexts;
using MysteryGuestAPI.DbContext;

namespace MysteryGuestAPI.Handlers.Authentication;

public record InviteUserRequest(string Email, Role Role);

public static class InviteUserHandler
{
    public static async Task<IResult> InviteUser(InviteUserRequest request, ApplicationDbContext context, ClaimsPrincipal claims, [FromServices] IConfiguration configuration)
    {
        if (claims.IsInRole(Role.Admin.ToString()) is false)
        {
            return TypedResults.Forbid();
        }
        
        var existingUser = await context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (existingUser is not null)
        {
            return TypedResults.BadRequest("User already exists");
        }
        var existingInvite = await context.UserInvites.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (existingInvite is not null)
        {
            context.UserInvites.Remove(existingInvite);
        }
        
        var userInvite = new UserInvite
        {
            Email = request.Email,
            ExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(configuration["UserInvite:TokenValidityInMinutes"]!)),
            Role = request.Role,
            Token = Guid.NewGuid().ToString()
        };

        await context.UserInvites.AddAsync(userInvite);
        await context.SaveChangesAsync();

        return TypedResults.Ok("User invited");
    }
}