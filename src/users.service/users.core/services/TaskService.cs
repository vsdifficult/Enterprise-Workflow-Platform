using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using users.core.repositories;
using users.core.services.interfaces;
using users.shared.dtos;
using users.shared.enums;

namespace users.core.services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ILogger<TaskService> _logger;

    public TaskService(ITaskRepository taskRepository, ILogger<TaskService> logger)
    {
        _taskRepository = taskRepository;
        _logger = logger;
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

        await _taskRepository.CreateAsync(task);
        return task;
    }

    public async Task<bool> DeleteTaskAsync(Guid id)
    {
        return await _taskRepository.DeleteAsync(id);
    }

    public async Task<TaskDto> GetTaskByIdAsync(Guid id)
    {
        return await _taskRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<TaskDto>> GetTasksByUserIdAsync(Guid userId)
    {
        return await _taskRepository.GetUserTasksAsync(userId);
    }

    public async Task<bool> UpdateTaskStatusAsync(Guid id, TskStatus status)
    {
        return await _taskRepository.ChangeTaskStatusAsync(id, status);
    }
}