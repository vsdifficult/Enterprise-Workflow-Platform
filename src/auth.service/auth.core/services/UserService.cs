

using auth.core.services.interfaces;
using auth.shared.dtos;
using auth.core.repositories;
using Microsoft.Extensions.Logging;

namespace auth.core.services;

public class UserService : IUserService
{
    private readonly IDataService _dataService;
    private readonly ILogger<UserService> _logger;

    public UserService(IDataService dataService, ILogger<UserService> logger)
    {
        _dataService = dataService;
        _logger = logger;
    }

    public async Task<UserDto> GetUserByIdAsync(Guid id)
    {
        return await _dataService.Users.GetByIdAsync(id);
    }

    public async Task<UserDto> GetUserByEmailAsync(string email)
    {
        return await _dataService.Users.GetByEmailAsync(email);
    }
}