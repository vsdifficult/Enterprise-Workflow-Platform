
namespace auth.shared.dtos; 

public record VerificationDto
{
    public string Email {get; init; } 
    public string Code {get; init; }
}