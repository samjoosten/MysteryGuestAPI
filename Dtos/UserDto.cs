using MysteryGuestAPI.Contexts;

namespace MysteryGuestAPI.Dtos;

public record UserDto
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public Role Role { get; init; }
    public required string FullName { get; init; }
}

public static class UserDtoExtensions
{
    public static UserDto ToDto(this ApplicationUser user)
    {
        return new UserDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role,
            FullName = user.FullName(),
        };
    }
}