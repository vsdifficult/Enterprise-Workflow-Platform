namespace notifications.shared.dtos;

public record SendEmailRequest
{
    public string? ToEmail { get; init; }
    public string? Subject { get; init; }
    public string? Body { get; init; }
}