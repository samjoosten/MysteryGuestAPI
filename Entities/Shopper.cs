namespace MysteryGuestAPI.Contexts;

public class Shopper : BaseEntity
{
    public string UserId { get; set; }
    
    public virtual ApplicationUser User { get; set; }
    public virtual ICollection<Visit> Visits { get; set; }
}