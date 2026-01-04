using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using users.infastructure.entities;

public class UsersDbContext: DbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options): base(options) {} 

    public DbSet<TaskEntity> Tasks {get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}