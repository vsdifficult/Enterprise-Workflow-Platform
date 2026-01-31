using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore.Design;

namespace notifications.infrastructure;

public class NotificationsDbContextFactory : IDesignTimeDbContextFactory<NotificationsDbContext>
{
    public NotificationsDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<NotificationsDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=enterprisenotifications;Username=postgres;Password=postgres");
        return new NotificationsDbContext(optionsBuilder.Options);
    }
}