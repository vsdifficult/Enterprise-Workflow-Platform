
namespace auth.shared.dtos; 

public record VerificationRequest
{
    public string? Email {get; init; } 
    public string? Code {get; init; }
}