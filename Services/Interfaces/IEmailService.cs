using MysteryGuestAPI.Dtos;

namespace MysteryGuestAPI.Services.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(EmailDto emailDto);
}