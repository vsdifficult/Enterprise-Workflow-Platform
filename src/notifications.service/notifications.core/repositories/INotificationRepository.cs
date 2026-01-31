using shared.repositories;
using notifications.shared.dtos;

namespace notifications.core.repositories;

public interface INotificationRepository : IRepository<NotificationDto, Guid>
{
    Task<IEnumerable<NotificationDto>> GetByUserIdAsync(Guid userId);
    Task<bool> MarkAsSentAsync(Guid id);
}