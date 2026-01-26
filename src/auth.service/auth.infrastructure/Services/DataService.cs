using auth.core.repositories; 
using auth.core.services.interfaces;
using auth.infrastructure.repositories;
using Microsoft.Extensions.Logging;

namespace auth.infrastructure.services;

public class DataService : IDataService
{
    private readonly AuthDbContext _context; 

    private readonly ILoggerFactory _loggerFactory; 

    public DataService(
        AuthDbContext context,
        ILoggerFactory loggerFactory
    )
    {
        _context = context; 
        _loggerFactory = loggerFactory; 

        Users = new UserRepository(_context, _loggerFactory.CreateLogger<UserRepository>()); 
    } 

    public IUserRepository Users {get; }

}