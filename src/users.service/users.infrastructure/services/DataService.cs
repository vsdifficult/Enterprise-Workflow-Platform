
using Microsoft.Extensions.Logging;
using users.core.repositories;
using users.core.services.interfaces;
using users.infrastructure.repositories;

namespace users.infrastructure.services; 

public class DataService: IDataService 
{
    private readonly UsersDbContext _context; 

    private readonly ILoggerFactory _loggerFactory; 

    public DataService(
        UsersDbContext context,
        ILoggerFactory loggerFactory
    )
    {
        _context = context; 
        _loggerFactory = loggerFactory; 

        Tasks = new TaskRepository(_context, _loggerFactory.CreateLogger<TaskRepository>()); 
    } 

    public ITaskRepository Tasks {get; }
}