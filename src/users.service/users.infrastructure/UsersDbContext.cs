using Microsoft.EntityFrameworkCore;
using users.infrastructure.entities;
using users.infrastructure.entities.configurations;

public class UsersDbContext: DbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options): base(options) {} 

    public DbSet<TaskEntity> Tasks {get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new TaskEntityFrameworkConfiguration());
    }
}
