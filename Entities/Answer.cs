namespace MysteryGuestAPI.Contexts;

public class Answer : BaseEntity
{
    public virtual Question Question { get; set; }
}

public class DateTimeAnswer : Answer
{
    public DateTime Value { get; set; }
}

public class ScoreAnswer : Answer
{
    public int Score { get; set; }
    public string? AdditionalRemarks { get; set; }
}

public class NumberAnswer : Answer
{
    public int Value { get; set; }
}

public class TextAnswer : Answer
{
    public string Value { get; set; }
}