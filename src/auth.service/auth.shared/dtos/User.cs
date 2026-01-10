

using auth.shared.enums;

namespace auth.shared.dtos; 

public record UserDto
{
    public Guid Id { get; set; } 

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }
    
    public string? Name { get; set; } 

    public string? PasswordHash { get; set; } 

    public string? Email { get; set; } 

    public bool Active {get; set; } 

    public string? Code {get; set; } 

    public bool IsVerify {get; set; } 

    public Role UserRole { get; set; } 
}