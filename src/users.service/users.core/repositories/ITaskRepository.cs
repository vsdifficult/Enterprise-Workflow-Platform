
using shared.repositories;
using users.shared.dtos;

namespace users.core.repositories; 

public interface ITaskRepository: IRepository<TaskDto, Guid>
{
    Task<IEnumerable<TaskDto?>> GetUserTasksAsync(Guid userId); 
}