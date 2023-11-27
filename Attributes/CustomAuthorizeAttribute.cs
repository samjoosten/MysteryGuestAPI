using Microsoft.AspNetCore.Authorization;
using MysteryGuestAPI.Contexts;

namespace MysteryGuestAPI.Attributes;

public class CustomAuthorizeAttribute : AuthorizeAttribute
{
    public CustomAuthorizeAttribute(Role role)
    {
        Roles = role.ToString().Replace(" ", string.Empty);
    }
}