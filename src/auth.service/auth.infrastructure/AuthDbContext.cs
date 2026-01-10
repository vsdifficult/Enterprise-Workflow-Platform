using auth.infrastructure.entites;
using auth.infrastructure.entites.configurations;
using Microsoft.EntityFrameworkCore; 

public class AuthDbContext: DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options): base(options) {}

    public DbSet<User> Users {get; set; } = null!; 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);  

        modelBuilder.ApplyConfiguration(new UserEntityFrameworkConfiguration()); 
    } 
}