using shared.entities; 
using auth.shared.enums; 

namespace auth.infrastructure.entites; 

public class User: BaseEntity
{
    public string? Name { get; set; } 

    public string? PasswordHash { get; set; } 

    public string? Email { get; set; } 

    public bool Active {get; set; }

    public string? Code {get; set; } 

    public bool IsVerify {get; set; }

    public Role UserRole { get; set; } 
} 
