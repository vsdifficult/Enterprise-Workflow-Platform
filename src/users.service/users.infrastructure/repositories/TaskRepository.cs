
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using users.core.repositories;
using users.infrastructure.entities;
using users.shared.dtos;
using users.shared.enums;

namespace users.infrastructure.repositories;

public class TaskRepository: ITaskRepository
{
    private readonly UsersDbContext _context;
    private readonly ILogger<TaskRepository> _logger;

    public TaskRepository(UsersDbContext context, ILogger<TaskRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Guid> CreateAsync(TaskDto dto)
    {
        try
        {
            var entity = MapToEntity(dto);
            await _context.Tasks.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating task {Username}", dto.Name);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            var tsk = await _context.Tasks.FindAsync(id);
            if (tsk == null) { return false; }

            _context.Tasks.Remove(tsk);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting task with ID {Id}", id);
            throw;
        }
    }

    public async Task<TaskDto?> GetByIdAsync(Guid id)
    {
        try
        {
            var tsk = await _context.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
            return tsk == null ? null : MapToDto(tsk);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving task with ID {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<TaskDto?>> GetUserTasksAsync(Guid userId)
    {
        try
        {
            var tsks = await _context.Tasks
                .Where(t => t.UserId.Contains(userId))
                .AsNoTracking()
                .Select(t => MapToDto(t)).ToListAsync();

            return tsks;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user task with user ID {Id}", userId);
            throw;
        }
    }

    public async Task<IEnumerable<TaskDto>> GetAllAsync()
    {
        try
        {
            var tsks = await _context.Tasks
                .Where(t => t.Status == TskStatus.Active)
                .AsNoTracking()
                .Select(t => MapToDto(t)).ToListAsync();

            return tsks;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error return tasks");
            throw;
        }
    }

    public async Task<bool> ChangeTaskStatusAsync(Guid id, TskStatus status)
    {
        try
        {
            var tsk = await _context.Tasks.FindAsync(id);
            if (tsk == null) { return false; }

            tsk.Status = status;
            _context.Tasks.Update(tsk);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error change task status");
            throw;
        }
    } 

    public async Task<bool> AddTaskToUserAsync(Guid taskId, Guid userId)
    {
        try
        {
            var tsk = await _context.Tasks.FindAsync(taskId);
            if (tsk == null) { return false; }

            tsk.UserId.Add(userId);
            _context.Tasks.Update(tsk);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding task to user");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(TaskDto entity)
    {
        try
        {
            var task = await _context.Tasks.FindAsync(entity.Id);
            if (task == null) return false;

            task.Name = entity.Name;
            task.Description = entity.Description;
            task.Status = entity.Status;
            task.UpdateAt = DateTime.UtcNow;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating task {entity.Id}");
            throw;
        }
    }

    private TaskDto MapToDto(TaskEntity body)
    {
        return new TaskDto
        {
            Id = body.Id,
            UserId = body.UserId,
            Name = body.Name,
            CreateAt = body.CreateAt,
            UpdateAt = body.UpdateAt,
            Description = body.Description,
            Status = body.Status
        };
    }

    private TaskEntity MapToEntity(TaskDto body)
    {
        return new TaskEntity
        {
            Id = body.Id,
            UserId = body.UserId,
            Name = body.Name,
            CreateAt = body.CreateAt,
            UpdateAt = body.UpdateAt,
            Description = body.Description,
            Status = body.Status
        };
    }
}
