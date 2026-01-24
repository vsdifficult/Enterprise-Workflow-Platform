
using shared.repositories;
using users.shared.dtos;
using users.shared.enums;

namespace users.core.repositories; 

public interface ITaskRepository: IRepository<TaskDto, Guid>
{
    Task<IEnumerable<TaskDto?>> GetUserTasksAsync(Guid userId); 

    Task<bool> ChangeTaskStatusAsync(Guid id, TskStatus status); 

    Task<bool> AddTaskToUserAsync(Guid taskId, Guid userId);
}