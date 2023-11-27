namespace MysteryGuestAPI.Handlers.Authentication.Shared;

public record AccessTokenResponse
{
    public required string AccessToken { get; init; }
    public required DateTime Expiration { get; init; }
    public required string RefreshToken { get; init; }
};