
using auth.infrastructure.entites;
using Microsoft.EntityFrameworkCore;

namespace auth.infrastructure.repositories; 

public class UserRepository: IUserRepository
{
    private readonly AuthDbContext _context;
    public UserRepository(AuthDbContext context)
    {
        _context = context; 
    } 

    public async Task<Guid> CreateAsync(User entity)
    {
        await _context.Users.AddAsync(entity); 
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
    public async Task<User?> GetByIdAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        return user;
    } 

    public async Task<User?> GetByEmailAsync(string email)
    {
        var usr = await _context.Users.
            AsNoTracking().
            FirstOrDefaultAsync(u => u.Email == email) ?? throw new Exception("User not found"); 
        
        return usr; 

    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users
            .Where(user => user.Active == true)
            .ToListAsync();
    } 

    public async Task<bool> UpdateAsync(User entity)
    {
        throw new NotImplementedException(); 
    }
}