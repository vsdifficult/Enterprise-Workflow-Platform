using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using auth.shared.dtos;

namespace auth.core.services.interfaces;

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
public interface IAuthService
{
    Task<AuthResult> SignUpAsync(RegisterRequest dto);
    Task<AuthResult> SignInAsync(LoginRequest dto);
    Task<AuthResult> VerificationAsync(VerificationRequest dto);
    Task<AuthResult> DeleteAsync(Guid userid);
}