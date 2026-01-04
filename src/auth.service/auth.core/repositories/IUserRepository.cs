using shared.repositories; 
using auth.shared.dtos; 

namespace auth.infrastructure.repositories; 

public interface IUserRepository: IRepository<UserDto, Guid>
{
    Task<UserDto?> GetByEmailAsync(string email);  
    Task<UserDto?> GetUserRoleAsync(Guid userId); 
}