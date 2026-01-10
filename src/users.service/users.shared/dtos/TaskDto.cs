using users.shared.enums; 

namespace users.shared.dtos; 

public record TaskDto
{
    public Guid Id {get; set; } 

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public Guid UserId {get; set; }

    public string? Name {get; set; } 

    public string? Description {get; set; } 
    
    public TskStatus Status {get; set; }
} 

public record CreateTaskDto
{
    public Guid UserId {get; set; }

    public string? Name {get; set; } 

    public string? Description {get; set; } 
}