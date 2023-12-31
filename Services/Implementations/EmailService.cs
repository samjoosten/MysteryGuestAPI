using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MysteryGuestAPI.Dtos;
using MysteryGuestAPI.Services.Interfaces;

namespace MysteryGuestAPI.Services.Implementations;

public class EmailService(IConfiguration configuration) : IEmailService
{
    public async Task SendEmailAsync(EmailDto emailDto)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(configuration.GetSection("EmailSettings")["From"]));
        email.To.Add(MailboxAddress.Parse(emailDto.To));
        
        email.Subject = emailDto.Subject;
        var builder = new BodyBuilder
        {
            HtmlBody = emailDto.Body
        };
        email.Body = builder.ToMessageBody();
        
        using (var smtp = new SmtpClient())
        {
            await smtp.ConnectAsync(configuration.GetSection("EmailSettings")["Host"], int.Parse(configuration.GetSection("EmailSettings")["Port"]!), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(configuration.GetSection("EmailSettings")["From"], configuration.GetSection("EmailSettings")["Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}