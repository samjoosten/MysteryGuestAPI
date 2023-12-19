using Microsoft.EntityFrameworkCore;
using MysteryGuestAPI.DbContext;

namespace MysteryGuestAPI.Handlers.Authentication;

public static class GetInviteByTokenHandler
{
    public static async Task<IResult> GetInvite(string token, ApplicationDbContext context)
    {
        var invite = await context.UserInvites.FirstOrDefaultAsync(x => x.Token == token);
        if (invite is null)
        {
            return TypedResults.NotFound("Invite not found");
        }

        if (!invite.IsValid())
        {
            return TypedResults.BadRequest("Invite is not valid");
        }

        return TypedResults.Ok(invite);
    }
}