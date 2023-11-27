namespace MysteryGuestAPI.Contexts;

public class Question : BaseEntity
{
    public string Name { get; set; }
    public QuestionType Type { get; set; }
    
    public virtual ICollection<Answer> Answers { get; set; }
}

public enum QuestionType
{
    Text,
    DateTime,
    MultipleChoice,
    SingleChoice,
    YesNo,
}