using shared.repositories; 
using auth.shared.dtos;
using auth.shared.enums;

namespace auth.core.repositories; 

public interface IUserRepository: IRepository<UserDto, Guid>
{
    Task<UserDto?> GetByEmailAsync(string email);  
    Task<Role?> GetUserRoleAsync(Guid userId);  
    Task<bool> SetEmailVerifiedAsync(string email);  

    Task<bool> SetVerificationCodeAsync(string email, string code); 
}