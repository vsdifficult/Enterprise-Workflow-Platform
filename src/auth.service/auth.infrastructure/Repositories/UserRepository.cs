using auth.core.repositories;
using auth.infrastructure.entites;
using auth.shared.dtos;
using auth.shared.enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using shared.repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace auth.infrastructure.repositories;

public class UserRepository: IUserRepository
{
    private readonly AuthDbContext _context;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(
        AuthDbContext context,
        ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Guid> CreateAsync(UserDto entity)
    {
        try
        {
            var userEntity = MapToEntity(entity);
            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();
            return userEntity.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Create user");
            throw;
        }
    }

    public async Task<Role?> GetUserRoleAsync(Guid userId)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            return user?.UserRole;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error get user {userId}");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error delete user {id}");
            throw;
        }
    }

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            return user == null ? null : MapToDto(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error get user {id}");
            throw;
        }
    }

    public async Task<UserDto?> GetByEmailAsync(string email)
    {
        try
        {
            var usr = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);

            return usr == null ? null : MapToDto(usr);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error get user with email {email}");
            throw;
        }
    }

    public async Task<bool> SetEmailVerifiedAsync(string email)
    {
        try
        {
            var usr = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (usr == null) return false;

            usr.IsVerify = true;

            _context.Update(usr);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error verify user with email {email}");
            throw;
        }
    }

    public async Task<bool> SetVerificationCodeAsync(string email, string code)
    {
        try
        {
            var usr = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (usr == null) return false;

            usr.Code = code;

            _context.Update(usr);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error set code in user profile with email {email}");
            throw;
        }
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _context.Users
            .Where(user => user.Active)
            .ToListAsync();
        return users.Select(MapToDto);
    }

    public async Task<bool> UpdateAsync(UserDto entity)
    {
        try
        {
            var user = await _context.Users.FindAsync(entity.Id);
            if (user == null) return false;

            user.Name = entity.Name;
            user.Email = entity.Email;
            user.PasswordHash = entity.PasswordHash;
            user.Active = entity.Active;
            user.UserRole = entity.UserRole;
            user.UpdateAt = DateTime.UtcNow;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, $"Error updating user {entity.Id}");
            throw;
        }
    }

    private User MapToEntity(UserDto dto)
    {
        return new User
        {
            Id = dto.Id,
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = dto.PasswordHash,
            Active = dto.Active,
            UserRole = dto.UserRole,
            UpdateAt = dto.UpdateAt,
            CreateAt = dto.CreateAt,
            Code = dto.Code,
            IsVerify = dto.IsVerify
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