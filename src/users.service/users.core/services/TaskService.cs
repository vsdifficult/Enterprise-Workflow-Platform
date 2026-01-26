
using System.Net.Http.Headers; 
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed; 
using Microsoft.Extensions.Configuration; 
using users.core.services.interfaces;
using users.shared.dtos;
using users.shared.enums;

namespace users.core.services;

public class TaskService : ITaskService
{
    private readonly IDataService _dataService;
    private readonly ILogger<TaskService> _logger;
    private readonly HttpClient _httpClient; 
    private readonly IDistributedCache _cache;  
    private readonly IConfiguration _configuration; 

    public TaskService(IDataService dataService, 
                        ILogger<TaskService> logger,
                        IDistributedCache cache,
                        HttpClient httpClient,
                        IConfiguration configuration)
    {
        _dataService = dataService;
        _logger = logger;
        _cache = cache; 
        _httpClient = httpClient; 
        _configuration = configuration;
    }

    public async Task<TaskDto> CreateTaskAsync(CreateTaskDto taskDto)
    {
        var task = new TaskDto
        {
            Id = Guid.NewGuid(),
            UserId = taskDto.UserId,
            Name = taskDto.Name,
            Description = taskDto.Description,
            Status = TskStatus.Active,
            CreateAt = DateTime.UtcNow,
            UpdateAt = DateTime.UtcNow
        };

        await _dataService.Tasks.CreateAsync(task);
        _logger.LogInformation($"Task {task.Id} added"); 
        return task;
    }

    public async Task<bool> DeleteTaskAsync(Guid id)
    {
        return await _dataService.Tasks.DeleteAsync(id);
    }

    public async Task<TaskDto> GetTaskByIdAsync(Guid id)
    {
        return await _dataService.Tasks.GetByIdAsync(id);
    }

    public async Task<IEnumerable<TaskDto>> GetTasksByUserIdAsync(Guid userId)
    {
        return await _dataService.Tasks.GetUserTasksAsync(userId);
    }

    public async Task<bool> UpdateTaskStatusAsync(Guid id, TskStatus status)
    {
        return await _dataService.Tasks.ChangeTaskStatusAsync(id, status);
    }  

    public async Task<bool> AddTaskForUserAsync(Guid userId, Guid taskId)
    {
        if (await UserExistsAsync(userId) == false)
        {
            _logger.LogError($"User with id {userId} not found"); 
            throw new Exception("User not found");
        }

        if (await _dataService.Tasks.GetByIdAsync(taskId) == null)
        {
            _logger.LogError($"Task with id {taskId} not found"); 
            throw new Exception("Task not found");
        } 

        await _dataService.Tasks.AddTaskToUserAsync(userId, taskId); 
        _logger.LogInformation($"Task with id {taskId} add user with id {userId}"); 

        return true; 
    } 

    private async Task<bool> UserExistsAsync(Guid id)
    {
        var cacheKey = $"user:{id}"; 
        var cached = await _cache.GetStringAsync(cacheKey); 
        
        if (cached == "exists")
        {
            return true; 
        } 

        try
        {
            var accessToken = _configuration["AccessServiceJWT"]; 
            var request = new HttpRequestMessage(HttpMethod.Get, $"http://auth-service/users/{id}/exists");
            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                await _cache.SetStringAsync(cacheKey, "exists", new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) });
                return true;
            }
            return false;
        } 
        catch (Exception ex)
        {
            _logger.LogError($"Error {ex.Message}"); 

            return false; 
        }
    }
}