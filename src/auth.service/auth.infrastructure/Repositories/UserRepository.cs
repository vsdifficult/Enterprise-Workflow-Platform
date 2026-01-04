
using auth.core.repositories;
using auth.infrastructure.entites;
using auth.shared.dtos;
using auth.shared.enums;
using Microsoft.EntityFrameworkCore;
using shared.repositories;

namespace auth.infrastructure.repositories; 

public class UserRepository: IUserRepository
{
    private readonly AuthDbContext _context;
    public UserRepository(AuthDbContext context)
    {
        _context = context; 
    } 

    public async Task<Guid> CreateAsync(UserDto entity)
    {
        await _context.Users.AddAsync(MapToEntity(entity)); 
        await _context.SaveChangesAsync(); 
        return entity.Id; 
    } 

    public async Task<Role?> GetUserRoleAsync(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        return user?.UserRole;
    }
    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    } 
    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id) 
            ?? throw new Exception($"User with id {id} not found");
        return MapToDto(user);
    } 

    public async Task<UserDto?> GetByEmailAsync(string email)
    {
        var usr = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email) 
                ?? throw new Exception($"User with {email} email not found"); 
        
        return MapToDto(usr); 

    }

    public async Task<bool> SetEmailVerifiedAsync(string email)
    {
        var usr = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email)
            ?? throw new Exception($"User with {email} email not found"); 
        
        usr.IsVerify = true; 

        _context.Update(usr); 
        await _context.SaveChangesAsync(); 
        return true; 
    } 

    public async Task<bool> SetVerificationCodeAsync(string email, string code)
    {
        var usr = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email)
            ?? throw new Exception($"User with {email} email not found");  

        usr.Code = code; 

        _context.Update(usr); 
        await _context.SaveChangesAsync(); 
        return true; 
    }
    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _context.Users
            .Where(user => user.Active == true)
            .ToListAsync();
        return users.Select(MapToDto);
    } 

    public async Task<bool> UpdateAsync(UserDto entity)
    {
        throw new NotImplementedException(); 
    } 

    private User MapToEntity(UserDto entity)
    {
        return new User
        {
            Id = entity.Id,
            Name = entity.Name,
            Email = entity.Email,
            PasswordHash = entity.PasswordHash,
            Active = entity.Active,
            UserRole = entity.UserRole,
            UpdateAt = entity.UpdateAt,
            CreateAt = entity.CreateAt,
            Code = entity.Code,
            IsVerify = entity.IsVerify
        }; 
    } 
    private UserDto MapToDto(User entity)
    {
        return new UserDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Email = entity.Email,
            PasswordHash = entity.PasswordHash,
            Active = entity.Active,
            UserRole = entity.UserRole,
            UpdateAt = entity.UpdateAt,
            CreateAt = entity.CreateAt,
            Code = entity.Code,
            IsVerify = entity.IsVerify
        }; 
    }

}