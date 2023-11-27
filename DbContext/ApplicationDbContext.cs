using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MysteryGuestAPI.Contexts;

namespace MysteryGuestAPI.DbContext;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext()
    {
        
    }

    public virtual DbSet<Company> Companies { get; set; }
    public virtual DbSet<QuestionForm> QuestionForms { get; set; }
    public virtual DbSet<Shopper> Shoppers { get; set; }
    public virtual DbSet<Visit> Visits { get; set; }
    public virtual DbSet<Question> Questions { get; set; }
    public virtual DbSet<DateTimeAnswer> DateTimeAnswers { get; set; }
    public virtual DbSet<NumberAnswer> NumberAnswers { get; set; }
    public virtual DbSet<TextAnswer> TextAnswers { get; set; }
    public virtual DbSet<ScoreAnswer> ScoreAnswers { get; set; }
    public virtual DbSet<UserInvite> UserInvites { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "host=ep-gentle-recipe-75487514.eu-central-1.aws.neon.tech; database=neondb; search path=neondb; port=5432; user id=sam.joosten90; password=uvKc8XowqGx0;");
    }
}