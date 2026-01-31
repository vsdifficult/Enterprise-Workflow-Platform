using notifications.core.services.interfaces;
using notifications.core.repositories;
using notifications.shared.dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace notifications.api.features;

public static class NotificationEndpoints
{
    public static void MapNotificationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/notifications");

        group.MapPost("/send-email", async ([FromBody] SendEmailRequest request, [FromServices] IEmailService emailService) =>
        {
            var result = await emailService.SendEmailAsync(request);
            return result ? Results.Ok("Email sent successfully") : Results.BadRequest("Failed to send email");
        }).WithTags("Notifications");

        group.MapGet("/", async ([FromServices] INotificationRepository notificationRepository) =>
        {
            var notifications = await notificationRepository.GetAllAsync();
            return Results.Ok(notifications);
        }).WithTags("Notifications");
    }
}