using MailKit.Net.Smtp;
using MimeKit;
using notifications.core.services.interfaces;
using notifications.shared.dtos;
using Microsoft.Extensions.Configuration;

namespace notifications.core.services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> SendEmailAsync(SendEmailRequest request)
    {
        try
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(emailSettings["FromName"], emailSettings["FromEmail"]));
            message.To.Add(new MailboxAddress("", request.ToEmail));
            message.Subject = request.Subject;

            message.Body = new TextPart("plain")
            {
                Text = request.Body
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(emailSettings["SmtpServer"], int.Parse(emailSettings["Port"]), MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(emailSettings["Username"], emailSettings["Password"]);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
}