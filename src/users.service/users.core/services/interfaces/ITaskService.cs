using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using users.shared.dtos;
using users.shared.enums;

namespace users.core.services.interfaces;

public interface ITaskService
{
    Task<IEnumerable<TaskDto>> GetTasksByUserIdAsync(Guid userId);
    Task<TaskDto> GetTaskByIdAsync(Guid id);
    Task<TaskDto> CreateTaskAsync(CreateTaskDto task);
    Task<bool> UpdateTaskStatusAsync(Guid id, TskStatus status);
    Task<bool> DeleteTaskAsync(Guid id); 
    Task<bool> AddTaskForUserAsync(Guid userId, Guid taskId); 
}