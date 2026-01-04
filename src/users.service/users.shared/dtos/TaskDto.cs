using users.shared.enums; 

namespace users.shared.dtos; 

public record TaskDto
{
    public Guid Id {get; set; } 

    public Guid UserId {get; set; }

    public string Name {get; set; } 

    public string Description {get; set; } 
    
    public TaskStatuses Status {get; set; }
} 

public record CreateTaskDto
{
    public Guid UserId {get; set; }

    public string Name {get; set; } 

    public string Description {get; set; } 
}