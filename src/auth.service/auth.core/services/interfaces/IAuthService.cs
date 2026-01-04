using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using auth.shared.dtos;
using auth.shared.enums; 

namespace HostMarket.Core.Services.Interfaces;

public record AuthResult
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
    public string? Message { get; init; }
    public string? Token { get; init; }
}

/// <summary>
/// Service for authentication operations
/// </summary>
public interface IAuthenticationService
{
    Task<AuthResult> SignUpAsync(RegisterDto dto);
    Task<AuthResult> SignInAsync(LoginDto dto);
    Task<AuthResult> VerificationAsync(VerificationDto dto);
    Task<AuthResult> DeleteAsync(Guid userid);
}