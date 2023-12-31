using System.Text;

namespace MysteryGuestAPI.Contexts;

public class InviteEmail
{
    public required string Link { get; init; }
    public required string To { get; init; }
    public required string From { get; init; }
    public required string Subject { get; init; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("<h1>Uitnodiging voor Mystery Guest</h1>");
        sb.Append("<p>U bent uitgenodigd om deel te nemen aan Mystery Guest. Klik op de onderstaande link om uw account aan te maken.</p>");
        sb.Append($"<a href=\"{Link}\">Klik hier om uw account aan te maken</a>");
        return sb.ToString();
    }
}