namespace MysteryGuestAPI.Contexts;

public class Company : BaseEntity
{
    public string Name { get; set; }
    public string AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    
    public virtual ICollection<QuestionForm> QuestionForms { get; set; }
}