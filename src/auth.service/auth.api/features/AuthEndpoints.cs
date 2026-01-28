using auth.core.services.interfaces;
using auth.shared.dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace auth.api.features;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/register", async ([FromBody] RegisterRequest request, [FromServices] IAuthService authService) =>
        {
            var result = await authService.SignUpAsync(request);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }).WithTags("Auth");

        group.MapPost("/login", async ([FromBody] LoginRequest request, [FromServices] IAuthService authService) =>
        {
            var result = await authService.SignInAsync(request);
            return result.Success ? Results.Ok(result) : Results.Unauthorized();
        }).WithTags("Auth");

        group.MapPost("/verify", async ([FromBody] VerificationRequest request, [FromServices] IAuthService authService) =>
        {
            var result = await authService.VerificationAsync(request);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }).WithTags("Auth"); 
        
        group.MapGet("/{id}/exists", async (Guid id, [FromServices] IUserService userService) =>
        {
            var result = await userService.GetUserByIdAsync(id); 
            return Results.Ok(result); 
        }); 
    }
}
