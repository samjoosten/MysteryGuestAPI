using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MysteryGuestAPI.Contexts;

public class BaseEntity
{
    public int Id { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedAt { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime UpdatedAt { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public bool Archived { get; set; }
}