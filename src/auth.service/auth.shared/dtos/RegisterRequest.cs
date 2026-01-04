
namespace auth.shared.dtos; 

public record RegisterDto
{
    public string Name { get; init; }
    public string Email { get; init; } 

    public string Password { get; init; } 
}