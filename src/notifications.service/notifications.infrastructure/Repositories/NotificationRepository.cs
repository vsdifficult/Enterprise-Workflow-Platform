using notifications.core.repositories;
using notifications.infrastructure.Entities;
using notifications.shared.dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using shared.repositories;

namespace notifications.infrastructure.repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly NotificationsDbContext _context;
    private readonly ILogger<NotificationRepository> _logger;

    public NotificationRepository(
        NotificationsDbContext context,
        ILogger<NotificationRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Guid> CreateAsync(NotificationDto entity)
    {
        try
        {
            var notificationEntity = MapToEntity(entity);
            await _context.Notifications.AddAsync(notificationEntity);
            await _context.SaveChangesAsync();
            return notificationEntity.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating notification");
            throw;
        }
    }

    public async Task<NotificationDto?> GetByIdAsync(Guid id)
    {
        try
        {
            var notification = await _context.Notifications.FindAsync(id);
            return notification != null ? MapToDto(notification) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting notification by id");
            throw;
        }
    }

    public async Task<IEnumerable<NotificationDto>> GetAllAsync()
    {
        try
        {
            var notifications = await _context.Notifications.ToListAsync();
            return notifications.Select(MapToDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all notifications");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(NotificationDto entity)
    {
        try
        {
            var notification = await _context.Notifications.FindAsync(entity.Id);
            if (notification == null) return false;

            MapToEntity(entity, notification);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating notification");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null) return false;

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting notification");
            throw;
        }
    }

    public async Task<IEnumerable<NotificationDto>> GetByUserIdAsync(Guid userId)
    {
        try
        {
            var notifications = await _context.Notifications.Where(n => n.UserId == userId).ToListAsync();
            return notifications.Select(MapToDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting notifications by user id");
            throw;
        }
    }

    public async Task<bool> MarkAsSentAsync(Guid id)
    {
        try
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null) return false;

            notification.IsSent = true;
            notification.SentAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking notification as sent");
            throw;
        }
    }

    private Notification MapToEntity(NotificationDto dto)
    {
        return new Notification
        {
            Id = dto.Id,
            UserId = dto.UserId,
            Type = dto.Type,
            Subject = dto.Subject,
            Message = dto.Message,
            IsSent = dto.IsSent,
            SentAt = dto.SentAt,
            CreateAt = dto.CreateAt,
            UpdateAt = dto.UpdateAt
        };
    }

    private void MapToEntity(NotificationDto dto, Notification entity)
    {
        entity.UserId = dto.UserId;
        entity.Type = dto.Type;
        entity.Subject = dto.Subject;
        entity.Message = dto.Message;
        entity.IsSent = dto.IsSent;
        entity.SentAt = dto.SentAt;
        entity.UpdateAt = DateTime.UtcNow;
    }

    private NotificationDto MapToDto(Notification entity)
    {
        return new NotificationDto
        {
            Id = entity.Id,
            UserId = entity.UserId,
            Type = entity.Type,
            Subject = entity.Subject,
            Message = entity.Message,
            IsSent = entity.IsSent,
            SentAt = entity.SentAt,
            CreateAt = entity.CreateAt,
            UpdateAt = entity.UpdateAt
        };
    }
}