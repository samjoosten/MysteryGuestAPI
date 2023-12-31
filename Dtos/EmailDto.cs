using MysteryGuestAPI.Contexts;

namespace MysteryGuestAPI.Dtos;

public record EmailDto
{
    public required string To { get; init; }
    public required string Subject { get; init; }
    public required string Body { get; init; }
}

public static class EmailDtoExtensions
{
    public static EmailDto ToDto(this InviteEmail inviteEmail)
    {
        return new EmailDto
        {
            To = inviteEmail.To,
            Subject = inviteEmail.Subject,
            Body = inviteEmail.ToString()
        };
    }
}

