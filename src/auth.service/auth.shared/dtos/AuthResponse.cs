

namespace auth.shared.dtos; 

public record AuthDto
{
    public bool Succes {get; init;} 
    
    public string Token {get; init;} 

    public string Message {get; init;} 

    public string ErrorMessage {get; init;}
}