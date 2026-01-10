

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using users.core.repositories;
using users.infastructure.entities;
using users.shared.dtos;
using users.shared.enums;

namespace users.infastructure.repositories; 

public class TaskRepository: ITaskRepository
{
    private readonly UsersDbContext _context;  

    private readonly ILogger<TaskRepository> _logger; 
    public TaskRepository(UsersDbContext context,
        ILogger<TaskRepository> logger)
    {
        _context = context; 
        _logger = logger; 
    }

    public async Task<Guid> CreateAsync(TaskDto dto)
    {
        try
        {
            var entity = MapToEntity(dto);
            entity.CreateAt = DateTime.UtcNow;
            
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
            var tsk = await _context.Tasks
                .AsNoTracking() 
                .FirstOrDefaultAsync(t => t.Id == id); 
            
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
            var tsk = await _context.Tasks  
                .AsNoTracking()   
                .FirstOrDefaultAsync(t => t.Id == id ) 
                ?? throw new Exception($"Error find task with ID {id}"); 

            return MapToDto(tsk); 
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
                .Where(t => t.UserId == userId)
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
                .Where(t => t.Status == shared.enums.TskStatus.Active)
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
            var tsk = await _context.Tasks
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id); 

            if (tsk == null) { return false; }

            if (tsk.Status != status)
            {
                tsk.Status = status;
                _context.Tasks.Update(tsk);
                await _context.SaveChangesAsync(); 
            } 
            return true; 
        } 
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error change task status");
            throw;
        }
    }
    public async Task<bool> UpdateAsync(TaskDto entity)
    {
        throw new NotImplementedException(); 
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