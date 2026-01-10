using shared.entities;
using users.shared.enums; 

namespace users.infastructure.entities; 

public class TaskEntity: BaseEntity
{
    public Guid UserId {get; set; }

    public string Name {get; set; } 

    public string Description {get; set; } 
    
    public TskStatus Status {get; set; }
} 
