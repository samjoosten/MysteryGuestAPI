using Microsoft.AspNetCore.Identity;

namespace MysteryGuestAPI.Contexts;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Role Role { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }

    public string FullName()
    {
        return $"{FirstName} {LastName}";
    }
}

public enum Role
{
    Admin,
    Shopper,
    Company
}