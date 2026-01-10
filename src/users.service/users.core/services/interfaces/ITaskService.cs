
using users.shared.dtos;
using users.shared.enums; 

namespace users.core.services.interfaces; 

public interface ITaskService
{
    Task<bool> StartAsync(CreateTaskDto taskBody); 

    Task<bool> StopAsync(Guid taskId); 
    
    Task<bool> ChangeStatusAsync(Guid taskId, TskStatus status); 
}