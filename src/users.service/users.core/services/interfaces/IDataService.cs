using users.core.repositories; 

namespace users.core.services.interfaces; 

public interface IDataService
{
    public ITaskRepository Tasks {get; }
}