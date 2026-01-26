using auth.core.repositories;
using auth.core.services.interfaces; 

namespace auth.core.services.interfaces; 

public interface IDataService
{
    public IUserRepository Users {get; }
}