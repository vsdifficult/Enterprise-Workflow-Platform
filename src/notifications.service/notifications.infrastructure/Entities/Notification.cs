using shared.entities;
using notifications.shared.enums;

namespace notifications.infrastructure.Entities;

public class Notification : BaseEntity
{
    public Guid UserId { get; set; }
    public NotificationType Type { get; set; }
    public string? Subject { get; set; }
    public string? Message { get; set; }
    public bool IsSent { get; set; }
    public DateTime? SentAt { get; set; }
}