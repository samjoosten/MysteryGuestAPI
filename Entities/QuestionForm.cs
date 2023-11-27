using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MysteryGuestAPI.Contexts;

public class QuestionForm : BaseEntity
{
    public string Name { get; set; }
    public bool Archived { get; set; }
    
    public int OwnerCompanyId { get; set; }
    public virtual Company OwnerCompany { get; set; }
}