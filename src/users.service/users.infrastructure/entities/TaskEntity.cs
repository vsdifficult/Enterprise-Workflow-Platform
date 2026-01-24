using shared.entities;
using users.shared.enums; 

namespace users.infrastructure.entities; 

public class TaskEntity: BaseEntity
{
    public List<Guid> UserId {get; set; } = new List<Guid>(); 

    public string Name {get; set; } 

    public string Description {get; set; } 
    
    public TskStatus Status {get; set; }
} 
