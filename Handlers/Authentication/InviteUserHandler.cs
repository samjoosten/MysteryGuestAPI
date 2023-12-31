using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MysteryGuestAPI.Attributes;
using MysteryGuestAPI.Contexts;
using MysteryGuestAPI.DbContext;
using MysteryGuestAPI.Dtos;
using MysteryGuestAPI.Services.Interfaces;

namespace MysteryGuestAPI.Handlers.Authentication;

public record InviteUserRequest(string Email, Role Role);

public static class InviteUserHandler
{
    [CustomAuthorize(Role.Admin)]
    public static async Task<IResult> InviteUser([FromBody] InviteUserRequest request, ApplicationDbContext context,
        ClaimsPrincipal claims,
        [FromServices] IConfiguration configuration, [FromServices] IEmailService emailService)
    {
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

        var emailDto = new EmailDto
        {
            To = request.Email,
            Subject = "Mystery Guest - Invitation",
            Body =
                $"You have been invited to Mystery Guest as a {request.Role}! Please use the following link to register: {configuration["UserInvite:BaseUrl"]!}/registreren?token={userInvite.Token}"
        };
        
        await emailService.SendEmailAsync(emailDto);

        return TypedResults.Ok(userInvite);
    }
}