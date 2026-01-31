using notifications.shared.enums;

namespace notifications.shared.dtos;

public record NotificationDto
{
    public Guid Id { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }
    public Guid UserId { get; set; }
    public NotificationType Type { get; set; }
    public string? Subject { get; set; }
    public string? Message { get; set; }
    public bool IsSent { get; set; }
    public DateTime? SentAt { get; set; }
}