using notifications.shared.dtos;

namespace notifications.core.services.interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(SendEmailRequest request);
}