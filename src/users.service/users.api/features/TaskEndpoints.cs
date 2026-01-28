using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using users.core.services.interfaces;
using users.shared.dtos;
using users.shared.enums; 
using shared.enums.roles; 
using Microsoft.AspNetCore.Authorization;
using users.core.services;

namespace users.api.features;

public static class TaskEndpoints
{
    public static void MapTaskEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tasks");

        group.MapGet("/user/{userId}", async (Guid userId, [FromServices] ITaskService taskService) =>
        {
            var tasks = await taskService.GetTasksByUserIdAsync(userId);
            return Results.Ok(tasks);
        }).WithTags("Tasks");

        group.MapGet("/{id}", async (Guid id, [FromServices] ITaskService taskService) =>
        {
            var task = await taskService.GetTaskByIdAsync(id);
            return task != null ? Results.Ok(task) : Results.NotFound();
        }).WithTags("Tasks");

        group.MapPost("/", async ([FromBody] CreateTaskDto task, [FromServices] ITaskService taskService) =>
        {
            var createdTask = await taskService.CreateTaskAsync(task);
            return Results.Created($"/api/tasks/{createdTask.Id}", createdTask);
        })
        .WithTags("Tasks")
        .RequireAuthorization(new AuthorizeAttribute { Roles = DomenRoles.Boss.ToString() });;

        group.MapPut("/{id}/status", async (Guid id, [FromBody] TskStatus status, [FromServices] ITaskService taskService) =>
        {
            var result = await taskService.UpdateTaskStatusAsync(id, status);
            return result ? Results.Ok() : Results.NotFound();
        }).WithTags("Tasks");

        group.MapDelete("/{id}", async (Guid id, [FromServices] ITaskService taskService) =>
        {
            var result = await taskService.DeleteTaskAsync(id);
            return result ? Results.NoContent() : Results.NotFound();
        })
        .WithTags("Tasks")
        .RequireAuthorization(new AuthorizeAttribute { Roles = DomenRoles.Boss.ToString() }); 

        group.MapPost("/add-task/{userId}/{taskId}", async (Guid userId, Guid taskId, [FromServices] ITaskService taskService) =>
        {
            var result = await taskService.AddTaskForUserAsync(userId, taskId); 
            return result ? Results.NoContent(): Results.Ok(result); 
        })
        .RequireAuthorization(new AuthorizeAttribute { Roles = DomenRoles.Boss.ToString() });
    }
}
