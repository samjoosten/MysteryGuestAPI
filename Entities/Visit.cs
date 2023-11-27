namespace MysteryGuestAPI.Contexts;

public class Visit : BaseEntity
{
    public DateTime FinalVisitDate { get; set; }
    public DateTime MinVisitDate { get; set; }
    public DateTime MaxVisitDate { get; set; }
    public string Name { get; set; }
    public int MaxVisitors { get; set; }
    public VisitType Type { get; set; }
    public virtual QuestionForm QuestionForm { get; set; }
}

public enum VisitType
{
    Dinner,
    Lunch,
    Cinema,
    Museum,
    Other,
}