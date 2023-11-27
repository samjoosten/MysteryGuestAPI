namespace MysteryGuestAPI.Contexts;

public class UserInvite : BaseEntity
{
    public DateTime ExpiresAt { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public Role Role { get; set; }
}