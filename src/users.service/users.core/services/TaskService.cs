
using users.shared.dtos;
using users.shared.enums; 
using users.core.services.interfaces; 

using Microsoft.Extensions.Logging; 

namespace users.core.services; 

public class TaskService: ITaskService
{
    private readonly IDataService _dataService; 

    private readonly Logger<TaskService> _logger;

    public TaskService(
        IDataService dataService,
        Logger<TaskService> logger
    )
    {
        _dataService = dataService;
        _logger = logger;
    } 

    public async Task<bool> StartAsync(CreateTaskDto taskBody)
    {
        var task_dto = new TaskDto
        {
           Id = Guid.NewGuid(),
           UserId = taskBody.UserId,
           Name = taskBody.Name,
           Description = taskBody.Description,
           CreateAt = DateTime.UtcNow,
           UpdateAt = DateTime.UtcNow
        };  

        await _dataService.Tasks.CreateAsync(task_dto); 

        return true; 
    } 

    public async Task<bool> StopAsync(Guid taskId)
    {
        var tsk = await _dataService.Tasks.GetByIdAsync(taskId); 

        if (tsk == null)
        {
            _logger.LogError($"Task with id {taskId} not found");
            return false; 
        } 

        await _dataService.Tasks.ChangeTaskStatusAsync(taskId, TskStatus.Paused); 

        return true; 
    } 


    public async Task<bool> ChangeStatusAsync(Guid taskId, 
                                            TskStatus status)
    {
        var tsk = await _dataService.Tasks.GetByIdAsync(taskId); 

        if (tsk == null)
        {
            _logger.LogError($"Task with id {taskId} not found");
            return false; 
        }  

        await _dataService.Tasks.ChangeTaskStatusAsync(taskId, status);

        return true; 
    }
} 